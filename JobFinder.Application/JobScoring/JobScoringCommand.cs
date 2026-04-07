using JobFinder.Domain.Entities;

namespace JobFinder.Application.JobScoring
{
    public class JobScoringCommand
    {
        public string Headline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string CvText { get; set; } = string.Empty;
        public ICollection<UserSkillArea> UserJobSkills { get; set; } = [];
    }
}
