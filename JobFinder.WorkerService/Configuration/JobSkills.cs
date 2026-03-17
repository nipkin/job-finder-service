namespace JobFinder.WorkerService.Configuration
{
    public class JobSkills
    {
        public List<string> CoreSkills { get; set; } = [];

        public List<string> FrontendSkills { get; set; } = [];

        public List<string> CmsSkills { get; set; } = [];

        public string CvText { get; set; } = string.Empty;
    }
}