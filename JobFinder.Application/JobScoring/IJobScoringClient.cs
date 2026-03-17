namespace JobFinder.Application.JobScoring
{
    public interface IJobScoringClient
    {
        Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
        Task<string> GenerateJsonAsync(string prompt, CancellationToken ct = default);
    }
}
