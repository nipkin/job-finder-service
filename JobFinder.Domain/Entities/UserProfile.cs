namespace JobFinder.Domain.Entities
{
    public class UserProfile
    {
        private UserProfile() { }

        public Guid Id { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public ICollection<UserSkillArea> UserSkills { get; set; } = [];
        public string CvText { get; set; } = string.Empty;
        public ICollection<JobPosting> JobPostings { get; set; } = [];

        public static UserProfile Create(string userName, string passwordHash)
        {
            return new UserProfile
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                PasswordHash = passwordHash
            };
        }
    }
}