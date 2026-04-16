using JobFinder.Application.UserProfile;
using JobFinder.Application.UserProfile.UserSkillAreas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobFinder.Api.Features.UserProfile
{
    [ApiController]
    [Authorize]
    [Route("api/userprofile")]
    public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken ct)
        {
            var result = await userProfileService.GetByIdAsync(UserId, ct);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Ok(result.Value);
        }

        [HttpPut("cv")]
        public async Task<IActionResult> UpdateCvText([FromBody]string cvText, CancellationToken ct)
        {
            if(string.IsNullOrEmpty(cvText))
                return BadRequest();

            var result = await userProfileService.UpdateUserCvAsync(UserId, cvText, ct);
            if(!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });

            return Ok(result.Value);
        }

        [HttpGet("cv")]
        public async Task<IActionResult> GetCvText(CancellationToken ct)
        {

            var result = await userProfileService.GetUserCvAsync(UserId, ct);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });

            return Ok(result.Value);
        }
    }
}