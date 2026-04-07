namespace JobFinder.Application.Auth
{
    public interface IAuthService
    {
        Task<Result<RegisterUserResult>> RegisterAsync(RegisterUserCommand request, CancellationToken ct = default);
        Task<Result<LoginUserResult>> LoginAsync(LoginUserCommand request, CancellationToken ct = default);
    }
}
