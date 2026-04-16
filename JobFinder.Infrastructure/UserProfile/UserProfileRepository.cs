using JobFinder.Application.Common;
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
                .Include(u => u.UserSkillAreas)
                    .ThenInclude(a => a.UserSkills)
                .Include(u => u.JobPostings)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<DomainEntities.UserProfile?> GetProfileShallowAsync(Guid userId, CancellationToken ct = default)
        {
            return await db.UserProfiles.FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<DomainEntities.UserProfile?> GetProfileWithSkillAreaAsync(Guid userId, Guid areaId, CancellationToken ct = default)
        {
            return await db.UserProfiles
                .Include(u => u.UserSkillAreas.Where(a => a.Id == areaId))
                    .ThenInclude(a => a.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<DomainEntities.UserProfile?> GetProfileWithSkillAreasAsync(Guid userId, CancellationToken ct = default)
        {
            return await db.UserProfiles
                .Include(u => u.UserSkillAreas)
                    .ThenInclude(a => a.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<IEnumerable<UserSkillArea>> GetUserSkillsAsync(Guid id, CancellationToken ct = default)
        {
            var userProfile = await db.UserProfiles
                .Include(u => u.UserSkillAreas)
                    .ThenInclude(a => a.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
            return userProfile?.UserSkillAreas ?? Enumerable.Empty<UserSkillArea>();
        }

        public async Task<string?> GetUserCvAsync(Guid id, CancellationToken ct = default)
        {
            var cv = await db.UserProfiles.Where(u => u.Id == id).Select(u => u.CvText).FirstOrDefaultAsync(ct);
            return cv;
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {
            await db.SaveChangesAsync(ct);
        }
    }
}
