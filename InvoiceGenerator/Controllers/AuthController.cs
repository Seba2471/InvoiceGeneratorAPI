using InvoiceGenerator.Entities;
using InvoiceGenerator.Persistence;
using InvoiceGenerator.Responses;
using InvoiceGenerator.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenRepository<User> _tokenRepository;
        public AuthController(UserManager<User> userManager, ITokenRepository<User> tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }


        [HttpPost]
        public async Task<IActionResult> Login(Requests.SignInUser request)
        {
            var validator = new SignInUserValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new NotValidateRequest(validatorResult.Errors));
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized(new AuthenticationError("LoginFailed", "Incorrect user name or password"));
            }

            var roles = await _userManager.GetRolesAsync(user);


            var accessToken = _tokenRepository.GenerateAccessToken(user, roles);

            var refreshToken = _tokenRepository.GenereateRefreshToken(user);

            await _tokenRepository.AddAsync(refreshToken);

            var response = new Responses.SignInUser
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshTokenValue
            };


            return Ok(response);
        }
    }
}
