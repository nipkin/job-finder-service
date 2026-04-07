using JobFinder.Application.UserProfile.UserSkillAreas;
using JobFinder.Application.UserProfile.UserSkills;

namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResult>> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
