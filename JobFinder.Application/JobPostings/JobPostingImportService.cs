using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using Microsoft.Extensions.Logging;

namespace JobFinder.Application.JobPostings
{
    public class JobPostingImportService(
        IJobPostingWriter writer, 
        IJobSearchApiClient client, 
        IJobScoringService scoringService,
        ILogger<JobPostingImportService> logger) : IJobPostingImportService
    {
        public async Task ImportJobPostingsAsync(ICollection<Domain.Entities.UserSkillArea> skillAreas, string cvText, IEnumerable<string> terms, CancellationToken ct = default)
        {
            logger.LogInformation("Import started at {time}", DateTimeOffset.Now);

            var ads = await FetchAllAdsAsync(terms, ct);
            var savedCount = await ProcessAdsAsync(skillAreas, cvText, ads, ct);

            var deleted = await writer.RemoveOutdatedAsync(ct);
            await writer.SaveAsync(ct);

            logger.LogInformation("Import finished at {time}. {newPosts} was added, {oldPosts} was deleted.",
                DateTimeOffset.Now, savedCount, deleted);
        }

        private async Task<List<JobSearchResult>> FetchAllAdsAsync(IEnumerable<string> terms, CancellationToken ct)
        {
            var tasks = terms.Select(term => client.SearchAsync(term, ct));
            var results = await Task.WhenAll(tasks);
            return results.SelectMany(ads => ads).DistinctBy(ad => ad.WebpageUrl).ToList();
        }

        private async Task<int> ProcessAdsAsync(ICollection<Domain.Entities.UserSkillArea> skillAreas, string cvText, IEnumerable<JobSearchResult> ads, CancellationToken ct)
        {
            var savedCount = 0;
            var adList = ads.ToList();
            var urls = adList.Select(a => a.WebpageUrl).ToHashSet();
            var existingUrls = await writer.GetExistingUrlsAsync(urls, ct);

            foreach (var ad in ads)
            {
                if (existingUrls.Contains(ad.WebpageUrl)) continue;

                var score = await scoringService.MatchesPromptScoreAsync(ToScoringRequest(skillAreas, cvText, ad), ct);
                var posting = JobPostingMapper.FromApi(ad, score);

                if (!posting.IsGoodMatch())
                {
                    logger.LogInformation("Not a good match: {headline}", ad.Headline);
                    continue;
                }

                writer.Add(posting);
                savedCount += 1;
                logger.LogInformation("Saved job: {headline}", posting.Headline);
            }

            return savedCount;
        }

        private static JobScoringRequest ToScoringRequest(ICollection<Domain.Entities.UserSkillArea> skillAreas, string cvText, JobSearchResult ad) => new()
        {
            Headline = ad.Headline,
            Description = ad.Description?.Text ?? string.Empty,
            CvText = cvText,
            UserJobSkills = skillAreas
        };
    }
}