using InvoiceGenerator.Persistence;
using InvoiceGenerator.Responses;
using InvoiceGenerator.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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


        [HttpPost("login", Name = "Login")]
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

            var response = new Responses.SignInUser
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshTokenValue
            };


            return Ok(response);
        }

        [HttpPost("register", Name = "Register")]

        public async Task<IActionResult> Register([FromBody] Requests.RegisterUser request)
        {
            var validator = new RegisterUserValidator();

            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new Responses.NotValidate(validatorResult.Errors));
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
    }
}
