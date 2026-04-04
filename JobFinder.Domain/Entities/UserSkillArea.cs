namespace JobFinder.Domain.Entities
{
    public class UserSkillArea
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserSkill> UserSkills { get; set; } = [];
        public int SkillWeight { get; set; }

        public string? AddSkill(string name)
        {
            if (UserSkills.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return $"'{name}' already exists in this skill area.";

            UserSkills.Add(new UserSkill { Id = Guid.NewGuid(), Name = name });
            return null;
        }

        public string? RemoveSkill(Guid skillId)
        {
            var skill = UserSkills.FirstOrDefault(s => s.Id == skillId);
            if (skill is null)
                return $"Skill '{skillId}' not found.";

            UserSkills.Remove(skill);
            return null;
        }

        public string? UpdateSkill(Guid skillId, string newName)
        {
            var skill = UserSkills.FirstOrDefault(s => s.Id == skillId);
            if (skill is null)
                return $"Skill '{skillId}' not found.";

            if (UserSkills.Any(s => s.Id != skillId && s.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                return $"'{newName}' already exists in this skill area.";

            skill.Name = newName;
            return null;
        }
    }
}
