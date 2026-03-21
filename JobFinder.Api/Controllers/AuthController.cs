using JobFinder.Api.Requests;
using JobFinder.Api.Services;
using JobFinder.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await authService.LoginAsync(
                    new LoginUserRequest(request.UserName, request.Password), ct);

                var token = tokenService.Generate(result.Id, result.UserName);

                Response.Cookies.Append("auth_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await authService.RegisterAsync(
                    new RegisterUserRequest(request.UserName, request.Password, request.ConfirmPassword), ct);

                return Created($"/api/users/{result.Id}", result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { ex.Message });
            }
        }
    }
}
