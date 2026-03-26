namespace JobFinder.Application.UserProfile
{
    public record UserProfileSkillRequest(Guid UserId, Guid SkillId, string Skill);
}