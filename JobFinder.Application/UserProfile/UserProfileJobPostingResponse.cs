namespace JobFinder.Application.UserProfile
{
    public record UserProfileJobPostingResponse(
        Guid Id,
        string Headline,
        string? Region,
        DateTime? ApplicationDeadline,
        string WebpageUrl,
        DateTime CreatedAtUtc,
        double? CvScore);
}
