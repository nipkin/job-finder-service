using JobFinder.Api.Requests;
using JobFinder.Api.Services;
using JobFinder.Application.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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

            var result = await authService.LoginAsync(
                new LoginUserRequest(request.UserName, request.Password), ct);

            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });

            var token = tokenService.Generate(result.Value!.Id, result.Value.UserName);

            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            return Ok(result.Value);
        }

        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            var token = Request.Cookies["auth_token"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { Message = "No auth token found." });

            var principal = tokenService.Validate(token);

            if (principal is null)
                return Unauthorized(new { Message = "Invalid or expired token." });

            var userId = Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
            var userName = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)!.Value;

            var newToken = tokenService.Generate(userId, userName);

            Response.Cookies.Append("auth_token", newToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            });

            return Ok(new { Valid = true });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await authService.RegisterAsync(
                new RegisterUserRequest(request.UserName, request.Password, request.ConfirmPassword),
                ct);

            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });

            return Created($"/api/users/{result.Value!.Id}", result.Value);
        }
    }
}
