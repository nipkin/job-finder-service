namespace JobFinder.Domain.Entities
{
    public class JobPosting
    {
        public Guid Id { get; set; }
        public string Headline { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? ApplicationDeadline { get; set; }
        public string WebpageUrl { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
        public string? OptimizedCv { get; set; }
        public double? CvScore { get; set; }

        public const double MinimumScore = 70;
        public bool IsGoodMatch() => CvScore >= MinimumScore;
    }
}
