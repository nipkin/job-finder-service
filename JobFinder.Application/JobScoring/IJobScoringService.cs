namespace JobFinder.Application.JobScoring
{
    public interface IJobScoringService
    {
        Task<int> MatchesPromptScoreAsync(JobScoringCommand ad, CancellationToken ct = default);
    }
}