namespace JobFinder.WorkerService.Configuration
{
    public class SearchOptions
    {
        public Guid UserProfileId { get; set; }
        public List<string> SearchTerms { get; set; } = [];
    }
}