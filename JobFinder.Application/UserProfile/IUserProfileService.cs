namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<UserProfileSkillAreaResponse?> CreateSkillArea(UserProfileSkillAreaRequest request);
        Task<string?> AddSkill(UserProfileSkillRequest request);
    }
}
