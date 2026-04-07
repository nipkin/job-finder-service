using JobFinder.Application.UserProfile.UserSkillAreas;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobFinder.Api.Features.UserProfile.UserSkillAreas
{
    [Route("api/userprofile")]
    [ApiController]
    public class UserSkillAreaController(IUserSkillAreaService userSkillAreaService) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("skillareas")]
        public async Task<IActionResult> CreateSkillAreaAsync(UserSkillAreaRequest request, CancellationToken ct)
        {
            var command = UserSkillAreaMappers.ToAddCommand(request);
            var result = await userSkillAreaService.CreateSkillAreaAsync(UserId, command);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Created($"/api/userprofile/skillareas/{result.Value!.Id}", result.Value);
        }

        [HttpPut("skillareas/{areaId:guid}")]
        public async Task<IActionResult> UpdateSkillAreaAsync(Guid areaId, UpdateUserSkillAreaRequest request, CancellationToken ct)
        {
            var command = UserSkillAreaMappers.ToUpdateCommand(request);
            var result = await userSkillAreaService.UpdateSkillAreaAsync(UserId, areaId, command);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return Ok(result.Value);
        }

        [HttpDelete("skillareas/{areaId:guid}")]
        public async Task<IActionResult> DeleteSkillAreaAsync(Guid areaId, CancellationToken ct)
        {
            var result = await userSkillAreaService.DeleteSkillAreaAsync(UserId, areaId);
            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });
            return NoContent();
        }
    }
}
