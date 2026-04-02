namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<UserProfileSkillAreaResponse>> CreateSkillArea(UserProfileSkillAreaRequest request);
        Task<Result<string>> AddSkill(UserProfileSkillRequest request);
    }
}
