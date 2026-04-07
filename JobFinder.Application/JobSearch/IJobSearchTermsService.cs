namespace JobFinder.Application.JobSearch
{
    public interface IJobSearchTermsService
    {
        public IEnumerable<string> GenerateTerms();
    }
}
