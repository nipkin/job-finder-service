namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResult>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<UserCvResult>> UpdateUserCvAsync(Guid id, string cvText, CancellationToken ct = default);
        Task<Result<UserCvResult>> GetUserCvAsync(Guid id, CancellationToken ct = default);
    }
}
