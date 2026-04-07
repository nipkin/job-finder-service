using JobFinder.Application.UserProfile.UserSkills;

namespace JobFinder.Application.UserProfile.UserSkillAreas
{
    public record UserSkillAreaResult(Guid Id, string Name, ICollection<UserSkillResult> Skills, int SkillWeight);
}
