using JobFinder.Application.UserProfile;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var result = await userProfileService.GetByIdAsync(id, ct);

            if (!result.IsSuccess)
                return StatusCode(result.Err!.StatusCode, new { result.Err.Message });

            return Ok(result.Value);
        }
    }
}
