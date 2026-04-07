using JobFinder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using JobFinder.Domain.Entities;
using JobFinder.Application.JobPostings;

namespace JobFinder.Infrastructure.JobPostings
{
    public class JobPostingReader(JobFinderDbContext db) : IJobPostingReader
    {
        public async Task<IReadOnlyList<JobPosting>> GetAllAsync()
        {
            return await db.JobPostings.ToListAsync();
        }
    }
}
