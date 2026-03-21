namespace JobFinder.Application.UserProfile
{
    public class UserProfileService(IUserProfileRepository repository) : IUserProfileService
    {
        public async Task<UserProfileResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var userProfile = await repository.GetByIdAsync(id, ct);

            if (userProfile is null)
                return null;

            return new UserProfileResponse(userProfile.Id, userProfile.UserName);
        }
    }
}