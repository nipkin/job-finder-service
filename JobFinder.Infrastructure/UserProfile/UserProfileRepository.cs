using JobFinder.Application.UserProfile;
using JobFinder.Domain.Entities;
using JobFinder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Infrastructure.UserProfile
{
    public class UserProfileRepository(JobFinderDbContext db) : IUserProfileRepository
    {
        public async Task<DomainEntities.UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await db.UserProfiles
                .Include(u => u.UserSkills)
                .Include(u => u.JobPostings)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {
            await db.SaveChangesAsync(ct);
        }
    }
}
