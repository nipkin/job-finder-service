namespace JobFinder.Domain.Entities
{
    public class UserSkillArea
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Skill> Skills { get; set; } = [];
        public int SkillWeight { get; set; }

        public string? AddSkill(string name)
        {
            if (Skills.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return $"'{name}' already exists in this skill area.";

            Skills.Add(new Skill { Id = Guid.NewGuid(), Name = name });
            return null;
        }

        public string? RemoveSkill(Guid skillId)
        {
            var skill = Skills.FirstOrDefault(s => s.Id == skillId);
            if (skill is null)
                return $"Skill '{skillId}' not found.";

            Skills.Remove(skill);
            return null;
        }

        public string? UpdateSkill(Guid skillId, string newName)
        {
            var skill = Skills.FirstOrDefault(s => s.Id == skillId);
            if (skill is null)
                return $"Skill '{skillId}' not found.";

            if (Skills.Any(s => s.Id != skillId && s.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                return $"'{newName}' already exists in this skill area.";

            skill.Name = newName;
            return null;
        }
    }
}
