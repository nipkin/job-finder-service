namespace JobFinder.Application.JobPostings
{
    public class UserJobSkills
    {
        public List<string> CoreSkills { get; set; } = [];

        public List<string> FrontendSkills { get; set; } = [];

        public List<string> CmsSkills { get; set; } = [];

        public string CvText { get; set; } = string.Empty;
    }
}
