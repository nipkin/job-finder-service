using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.Auth
{
    public class AuthService(IAuthRepository repository) : IAuthService
    {
        private const int MinPasswordLength = 4;
        private const int MaxUserNameLength = 50;
        private static readonly PasswordHasher<DomainEntities.UserProfile> _hasher = new();

        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default)
        {
            ValidateRequest(request);

            var existing = await repository.GetByUserNameAsync(request.UserName, ct);
            if (existing is not null)
                throw new InvalidOperationException($"Username '{request.UserName}' is already taken.");

            var passwordHash = _hasher.HashPassword(null!, request.Password);
            var userProfile = DomainEntities.UserProfile.Create(request.UserName, passwordHash);

            repository.Add(userProfile);
            await repository.SaveAsync(ct);

            return new RegisterUserResponse(userProfile.Id, userProfile.UserName);
        }

        public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ArgumentException("Username is required.", nameof(request.UserName));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.", nameof(request.Password));

            var userProfile = await repository.GetByUserNameAsync(request.UserName, ct);
            if(userProfile is null)
                throw new UnauthorizedAccessException("Invalid username.");
            var result = _hasher.VerifyHashedPassword(userProfile, userProfile?.PasswordHash ?? string.Empty, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid username or password.");
            
            if(userProfile == null)
                return null!;

            return new LoginUserResponse(userProfile.Id, userProfile.UserName);
        }

        private static void ValidateRequest(RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ArgumentException("Username is required.", nameof(request.UserName));

            if (request.UserName.Length > MaxUserNameLength)
                throw new ArgumentException($"Username must not exceed {MaxUserNameLength} characters.", nameof(request.UserName));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.", nameof(request.Password));

            if (request.Password.Length < MinPasswordLength)
                throw new ArgumentException($"Password must be at least {MinPasswordLength} characters.", nameof(request.Password));

            if (request.Password != request.ConfirmPassword)
                throw new ArgumentException("Password and confirmation password do not match.", nameof(request.ConfirmPassword));
        }
    }
}
