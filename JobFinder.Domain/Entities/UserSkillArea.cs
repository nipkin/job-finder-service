namespace JobFinder.Domain.Entities
{
    public class UserSkillArea
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<string> Skill { get; set; } = [];
        public int SkillWeight { get; set; }
    }
}