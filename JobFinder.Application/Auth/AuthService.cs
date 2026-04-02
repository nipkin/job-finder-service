using Microsoft.AspNetCore.Identity;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.Auth
{
    public class AuthService(IAuthRepository repository) : IAuthService
    {
        private const int MinPasswordLength = 4;
        private const int MaxUserNameLength = 50;
        private static readonly PasswordHasher<DomainEntities.UserProfile> _hasher = new();

        public async Task<Result<RegisterUserResponse>> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default)
        {
            var validation = ValidateRequest(request);
            if (!validation.IsSuccess)
                return validation.Err!;

            var existing = await repository.GetByUserNameAsync(request.UserName, ct);
            if (existing is not null)
                return Error.Conflict($"Username '{request.UserName}' is already taken.");

            var passwordHash = _hasher.HashPassword(null!, request.Password);
            var userProfile = DomainEntities.UserProfile.Create(request.UserName, passwordHash);

            repository.Add(userProfile);
            await repository.SaveAsync(ct);

            return new RegisterUserResponse(userProfile.Id, userProfile.UserName);
        }

        public async Task<Result<LoginUserResponse>> LoginAsync(LoginUserRequest request, CancellationToken ct = default)
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

            return new LoginUserResponse(userProfile.Id, userProfile.UserName);
        }

        private static Result<bool> ValidateRequest(RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                return Error.Validation("Username is required.");

            if (request.UserName.Length > MaxUserNameLength)
                return Error.Validation($"Username must not exceed {MaxUserNameLength} characters.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Error.Validation("Password is required.");

            if (request.Password.Length < MinPasswordLength)
                return Error.Validation($"Password must be at least {MinPasswordLength} characters.");

            if (request.Password != request.ConfirmPassword)
                return Error.Validation("Passwords do not match.");

            return true;
        }
    }
}
