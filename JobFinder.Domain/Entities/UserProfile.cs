namespace JobFinder.Domain.Entities
{
    public class UserProfile
    {
        private const int MaxUserNameLength = 50;

        private UserProfile() { }

        public Guid Id { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public ICollection<UserSkillArea> UserSkills { get; set; } = [];
        public string CvText { get; set; } = string.Empty;
        public ICollection<JobPosting> JobPostings { get; set; } = [];

        public static (UserProfile? Profile, string? Error) Create(string userName, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return (null, "Username is required.");

            if (userName.Length > MaxUserNameLength)
                return (null, $"Username must not exceed {MaxUserNameLength} characters.");

            return (new UserProfile { Id = Guid.NewGuid(), UserName = userName, PasswordHash = passwordHash }, null);
        }
    }
}
