namespace JobFinder.Domain.Entities
{
    public class UserProfile
    {
        private const int MaxUserNameLength = 50;

        private UserProfile() { }

        public Guid Id { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public ICollection<UserSkillArea> UserSkillAreas { get; set; } = [];
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

        public string? UpdateSkillArea(Guid areaId, string name, int skillWeight)
        {
            if (skillWeight < 1 || skillWeight > 5)
                return "Skill weight must be between 1 and 5.";

            var area = UserSkillAreas.FirstOrDefault(a => a.Id == areaId);
            if (area is null)
                return $"Skill area '{areaId}' not found.";

            if (UserSkillAreas.Any(a => a.Id != areaId && a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return $"Skill area '{name}' already exists.";

            area.Name = name;
            area.SkillWeight = skillWeight;
            return null;
        }

        public (UserSkillArea? Area, string? Error) AddSkillArea(string name, int skillWeight)
        {
            if (UserSkillAreas.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return (null, $"Skill area '{name}' already exists.");

            var area = new UserSkillArea { Id = Guid.NewGuid(), Name = name, SkillWeight = skillWeight };
            UserSkillAreas.Add(area);
            return (area, null);
        }

        public string? RemoveSkillArea(Guid areaId)
        {
            var area = UserSkillAreas.FirstOrDefault(a => a.Id == areaId);
            if (area is null)
                return $"Skill area '{areaId}' not found.";

            UserSkillAreas.Remove(area);
            return null;
        }
    }
}
