using Microsoft.AspNetCore.Mvc;
using JobFinder.Application.JobPostings;

namespace JobFinderApi.Controllers
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
