using Microsoft.AspNetCore.Identity;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.Auth
{
    public class AuthService(IAuthRepository repository) : IAuthService
    {
        private const int MinPasswordLength = 4;
        private static readonly PasswordHasher<DomainEntities.UserProfile> _hasher = new();

        public async Task<Result<RegisterUserResult>> RegisterAsync(RegisterUserCommand request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return Error.Validation("Password is required.");

            if (request.Password.Length < MinPasswordLength)
                return Error.Validation($"Password must be at least {MinPasswordLength} characters.");

            if (request.Password != request.ConfirmPassword)
                return Error.Validation("Passwords do not match.");

            var existing = await repository.GetByUserNameAsync(request.UserName, ct);
            if (existing is not null)
                return Error.Conflict($"Username '{request.UserName}' is already taken.");

            var passwordHash = _hasher.HashPassword(null!, request.Password);
            var (userProfile, error) = DomainEntities.UserProfile.Create(request.UserName, passwordHash);
            if (error is not null) return Error.Validation(error);

            repository.Add(userProfile!);
            await repository.SaveAsync(ct);

            return new RegisterUserResult(userProfile!.Id, userProfile.UserName);
        }

        public async Task<Result<LoginUserResult>> LoginAsync(LoginUserCommand request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return Error.Validation("Username is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Error.Validation("Password is required.");

            var userProfile = await repository.GetByUserNameAsync(request.UserName, ct);
            if (userProfile is null)
                return Error.Unauthorized("Invalid username or password.");

            var result = _hasher.VerifyHashedPassword(userProfile, userProfile.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Error.Unauthorized("Invalid username or password.");

            return new LoginUserResult(userProfile.Id, userProfile.UserName);
        }
    }
}
