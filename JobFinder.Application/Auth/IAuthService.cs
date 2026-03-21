namespace JobFinder.Application.Auth
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default);
        Task<LoginUserResponse> LoginAsync(LoginUserRequest request, CancellationToken ct = default);
    }
}
