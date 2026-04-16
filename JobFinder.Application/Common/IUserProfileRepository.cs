using JobFinder.Domain.Entities;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.Common
{
    public interface IUserProfileRepository
    {
        Task<DomainEntities.UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<DomainEntities.UserProfile?> GetProfileShallowAsync(Guid userId, CancellationToken ct = default);
        Task<DomainEntities.UserProfile?> GetProfileWithSkillAreaAsync(Guid userId, Guid areaId, CancellationToken ct = default);
        Task<DomainEntities.UserProfile?> GetProfileWithSkillAreasAsync(Guid userId, CancellationToken ct = default);
        Task<IEnumerable<UserSkillArea>> GetUserSkillsAsync(Guid id, CancellationToken ct = default);
        Task<string?> GetUserCvAsync(Guid id, CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
    }
}
