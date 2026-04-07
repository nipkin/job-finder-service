namespace JobFinder.Application.JobPostings
{
    public class JobPostingResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Headline { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? ApplicationDeadline { get; set; }
        public string WebpageUrl { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string? OptimizedCv { get; set; }
        public double? CvScore { get; set; }
    }
}