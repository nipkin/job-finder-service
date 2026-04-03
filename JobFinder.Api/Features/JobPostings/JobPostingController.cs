using Microsoft.AspNetCore.Mvc;
using JobFinder.Application.JobPostings;

namespace JobFinder.Api.Features.JobPostings
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostingController(IJobPostingProviderService provider) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await provider.GetAllPostingsAsync();
            return Ok(results);
        }
    }
}
