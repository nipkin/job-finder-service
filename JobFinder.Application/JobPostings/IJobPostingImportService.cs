namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingImportService
    {
        Task ImportJobPostingsAsync(ICollection<Domain.Entities.UserSkillArea> skillAreas, string cvText, IEnumerable<string> terms, CancellationToken ct = default);
    }
}
