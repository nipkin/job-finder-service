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

        public async Task<IReadOnlyList<JobPosting>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await db.JobPostings
                .Where(j => j.UserProfileId == userId)
                .OrderByDescending(j => j.CreatedAtUtc)
                .ToListAsync(ct);
        }
    }
}
