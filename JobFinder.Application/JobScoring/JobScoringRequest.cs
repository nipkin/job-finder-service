using JobFinder.Application.JobPostings;

namespace JobFinder.Application.JobScoring
{
    public class JobScoringRequest
    {
        public string Headline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public UserJobSkills UserJobSkills { get; set; } = new UserJobSkills();
    }
}
