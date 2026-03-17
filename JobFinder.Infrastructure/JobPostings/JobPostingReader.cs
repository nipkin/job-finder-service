using JobFinder.Application.JobPostings;
using JobFinder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JobFinder.Domain.Entities;

namespace JobFinder.Infrastructure.JobPostings
{
    public class JobPostingReader(JobFinderDbContext db) : IJobPostingReader
    {
        public async Task<List<JobPosting>> GetAllAsync()
        {
            var result = await db.JobPostings.ToListAsync();
            return result;
        }
    }
}
