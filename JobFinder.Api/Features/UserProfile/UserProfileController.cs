using JobFinder.Application.UserProfile;
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

        [HttpPost("skillareas")]
        public async Task<IActionResult> CreateSkillAreaAsync(UserProfileSkillAreaRequest request, CancellationToken ct)
        {
            var result = await userProfileService.CreateSkillArea(UserId, request);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Created($"/api/userprofile/skillareas/{result.Value!.Id}", result.Value);
        }

        [HttpDelete("skillareas/{areaId:guid}")]
        public async Task<IActionResult> DeleteSkillAreaAsync(Guid areaId, CancellationToken ct)
        {
            var result = await userProfileService.DeleteSkillArea(UserId, areaId);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return NoContent();
        }

        [HttpPost("skillareas/{areaId:guid}/skills")]
        public async Task<IActionResult> AddSkillAsync(Guid areaId, AddSkillRequest request, CancellationToken ct)
        {
            var result = await userProfileService.AddSkill(UserId, areaId, request.Name);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Created($"/api/userprofile/skills/{result.Value!.Id}", result.Value);
        }

        [HttpPut("skills/{skillId:guid}")]
        public async Task<IActionResult> UpdateSkillAsync(Guid skillId, UpdateSkillRequest request, CancellationToken ct)
        {
            var result = await userProfileService.UpdateSkill(UserId, skillId, request.Name);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Ok(result.Value);
        }

        [HttpDelete("skills/{skillId:guid}")]
        public async Task<IActionResult> RemoveSkillAsync(Guid skillId, CancellationToken ct)
        {
            var result = await userProfileService.RemoveSkill(UserId, skillId);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return NoContent();
        }
    }
}