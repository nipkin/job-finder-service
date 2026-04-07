using JobFinder.Application.UserProfile.UserSkills;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobFinder.Api.Features.UserProfile.UserSkills
{
    [Route("api/userprofile")]
    [ApiController]
    public class UserSkillController(IUserSkillService userSkillService) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("skillareas/{areaId:guid}/skills")]
        public async Task<IActionResult> AddSkillAsync(Guid areaId, AddSkillRequest request, CancellationToken ct)
        {
            var result = await userSkillService.AddSkill(UserId, areaId, request.Name);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Created($"/api/userprofile/skills/{result.Value!.Id}", result.Value);
        }

        [HttpPut("skills/{skillId:guid}")]
        public async Task<IActionResult> UpdateSkillAsync(Guid skillId, UpdateSkillRequest request, CancellationToken ct)
        {
            var result = await userSkillService.UpdateSkill(UserId, skillId, request.Name);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Ok(result.Value);
        }

        [HttpDelete("skills/{skillId:guid}")]
        public async Task<IActionResult> RemoveSkillAsync(Guid skillId, CancellationToken ct)
        {
            var result = await userSkillService.RemoveSkill(UserId, skillId);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return NoContent();
        }
    }
}
