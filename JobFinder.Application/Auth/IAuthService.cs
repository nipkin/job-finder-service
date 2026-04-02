namespace JobFinder.Application.Auth
{
    public interface IAuthService
    {
        Task<Result<RegisterUserResponse>> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default);
        Task<Result<LoginUserResponse>> LoginAsync(LoginUserRequest request, CancellationToken ct = default);
    }
}
