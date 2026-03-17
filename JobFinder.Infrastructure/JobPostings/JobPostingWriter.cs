using JobFinder.Application.JobPostings;
using JobFinder.Domain.Entities;
using JobFinder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Infrastructure.JobPostings
{
    public class JobPostingWriter(JobFinderDbContext db) : IJobPostingWriter
    {
        public void Remove(JobPosting posting)
        {
            db.JobPostings.Remove(posting);
        }

        public void Add(JobPosting posting)
        {
            db.JobPostings.Add(posting);
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {     
            await db.SaveChangesAsync(ct);
        }

        public async Task<int> RemoveOutdatedAsync(CancellationToken ct = default)
        {
            var today = DateTime.Today;
            var outdatedPosts = await db.JobPostings
                .Where(x => x.ApplicationDeadline < today)
                .ToListAsync(ct);

            foreach (var post in outdatedPosts)
            {
                Remove(post);
            }

            return outdatedPosts.Count;
        }

        public async Task<bool> JobPostExists(string postingUrl, CancellationToken ct = default)
        {
            return await db.JobPostings.AnyAsync(x => x.WebpageUrl == postingUrl, ct);
        }
    }
}
