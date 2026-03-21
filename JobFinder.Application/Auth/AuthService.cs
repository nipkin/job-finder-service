using System.Security.Cryptography;
using System.Text;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.Auth
{
    public class AuthService(IAuthRepository repository) : IAuthService
    {
        private const int MinPasswordLength = 4;
        private const int MaxUserNameLength = 50;

        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default)
        {
            ValidateRequest(request);

            var existing = await repository.GetByUserNameAsync(request.UserName, ct);
            if (existing is not null)
                throw new InvalidOperationException($"Username '{request.UserName}' is already taken.");

            var passwordHash = HashPassword(request.Password);
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
            if (userProfile is null || !VerifyPassword(request.Password, userProfile.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

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

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations: 100_000,
                HashAlgorithmName.SHA256,
                outputLength: 32);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);

            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations: 100_000,
                HashAlgorithmName.SHA256,
                outputLength: 32);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
