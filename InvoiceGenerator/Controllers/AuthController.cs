using InvoiceGenerator.Persistence;
using InvoiceGenerator.Requests;
using InvoiceGenerator.Responses;
using InvoiceGenerator.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository<IdentityUser> _tokenRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository<IdentityUser> tokenRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _roleManager = roleManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(Requests.SignInUser request)
        {
            var validator = new SignInUserValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new NotValidate(validatorResult.Errors));
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized(new AuthenticationError("LoginFailed", "Incorrect user name or password"));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _tokenRepository.GenerateAccessToken(user, roles);

            var refreshToken = _tokenRepository.GenereateRefreshToken(user, Request.Headers.UserAgent);

            await _tokenRepository.AddAsync(refreshToken);

            var response = new Responses.Tokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshTokenValue
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Requests.RegisterUser request)
        {
            var validator = new RegisterUserValidator();

            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new NotValidate(validatorResult.Errors));
            }

            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var role = _roleManager.FindByNameAsync("User").Result;

                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                return Ok($"User {user.Email} created!");
            }

            if (result.Errors is not null)
            {
                return BadRequest(new AuthenticationError(result.Errors.ToList()));
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken request)
        {
            var userAgent = Request.Headers.UserAgent;

            var validator = new RefreshTokenValidator();

            var validatorResult = await validator.ValidateAsync(request);
            if (!validatorResult.IsValid)
            {
                return BadRequest(new NotValidate(validatorResult.Errors));
            }

            var tokenIsValid = _tokenRepository.ValidateRefreshToken(request.Token);

            if (!tokenIsValid)
            {
                return Unauthorized(new AuthenticationError("Token", Messages.TokenNotValid));
            }

            var tokenFromDb = await _tokenRepository.GetByTokenValue(request.Token);

            if (tokenFromDb == null || !tokenFromDb.Active || tokenFromDb.UserAgent != userAgent)
            {
                return Unauthorized(new AuthenticationError("Token", Messages.Unauthorized));
            }

            var user = new IdentityUser() { Id = tokenFromDb.UserId, UserName = tokenFromDb.UserName };

            var userRoles = await _userManager.GetRolesAsync(user);

            var accessToken = _tokenRepository.GenerateAccessToken(user, userRoles);

            var refreshToken = _tokenRepository.GenereateRefreshToken(user, userAgent);

            await _tokenRepository.DeleteAsync(tokenFromDb);

            await _tokenRepository.AddAsync(refreshToken);

            var response = new Responses.Tokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshTokenValue
            };

            return Ok(response);
        }
    }
}