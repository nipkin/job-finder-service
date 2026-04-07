namespace JobFinder.Application.UserProfile.UserSkillAreas
{
    public record AddUserSkillAreaCommand(string Name, ICollection<string> Skills, int SkillWeight);
}
