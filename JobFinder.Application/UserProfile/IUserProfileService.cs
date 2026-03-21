namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
