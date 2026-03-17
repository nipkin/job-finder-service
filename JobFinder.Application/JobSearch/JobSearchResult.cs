namespace JobFinder.Application.JobSearch
{
    public class JobSearchResult
    {
        public string Id { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string WebpageUrl { get; set; } = string.Empty;
        public DateTime? ApplicationDeadline { get; set; }
        public int NumberOfVacancies { get; set; }
        public double Relevance { get; set; }
        public JobDescription? Description { get; set; }
        public WorkplaceAddress? WorkplaceAddress { get; set; }
    }

    public class WorkplaceAddress
    {
        public string Region { get; set; } = string.Empty;
    }

    public class JobDescription
    {
        public string Text { get; set; } = string.Empty;
    }
}
