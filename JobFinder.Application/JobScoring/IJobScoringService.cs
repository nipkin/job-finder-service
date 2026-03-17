namespace JobFinder.Application.JobScoring
{
    public interface IJobScoringService
    {
        Task<int> MatchesPromptScoreAsync(JobScoringRequest ad, CancellationToken ct = default);
    }
}