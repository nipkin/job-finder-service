using JobFinder.Application.Common;
using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using Microsoft.Extensions.Logging;

namespace JobFinder.Application.JobPostings
{
    public class JobPostingImportService(
        IJobPostingWriter writer,
        IJobSearchApiClient client,
        IJobScoringService scoringService,
        IJobSearchTermsService termsService,
        IUserProfileRepository profileRepository,
        ILogger<JobPostingImportService> logger) : IJobPostingImportService
    {
        public async Task ImportJobPostingsAsync(Guid userId, IProgress<string>? progress = null, CancellationToken ct = default)
        {
            logger.LogInformation("Import started at {time}", DateTimeOffset.Now);
            progress?.Report("Fetching job listings...");

            var userProfile = await profileRepository.GetByIdAsync(userId, ct);
            if(userProfile == null) {
                logger.LogError("User not found: {userId}", userId);
                return;
            }
            var termsResult = await termsService.GenerateSearchTermsAsync(userProfile.UserSkillAreas, ct);
            var skillAreas = userProfile.UserSkillAreas;
            var cvText = userProfile.CvText;
            if (!termsResult.IsSuccess || termsResult.Value == null)
            {
                logger.LogError("Failed to generate search terms: {error}", termsResult.Err);
                return;
            }

            var terms = termsResult.Value;
            var ads = await FetchAllAdsAsync(terms, ct);
            var savedCount = await ProcessAdsAsync(userId, skillAreas, cvText, ads, progress, ct);

            var deleted = await writer.RemoveOutdatedAsync(ct);
            await writer.SaveAsync(ct);

            var summary = $"Done. {savedCount} job(s) saved, {deleted} outdated removed.";
            logger.LogInformation("Import finished at {time}. {newPosts} was added, {oldPosts} was deleted.", DateTimeOffset.Now, savedCount, deleted);
            progress?.Report(summary);
        }

        private async Task<List<JobSearchResult>> FetchAllAdsAsync(IEnumerable<string> terms, CancellationToken ct)
        {
            var tasks = terms.Select(term => client.SearchAsync(term, ct));
            var results = await Task.WhenAll(tasks);
            return results.SelectMany(ads => ads).DistinctBy(ad => ad.WebpageUrl).ToList();
        }

        private async Task<int> ProcessAdsAsync(Guid userId, ICollection<Domain.Entities.UserSkillArea> skillAreas, string cvText, List<JobSearchResult> ads, IProgress<string>? progress, CancellationToken ct)
        {
            var urls = ads.Select(a => a.WebpageUrl).ToHashSet();
            var existingUserUrls = await writer.GetExistingUserUrlsAsync(userId, urls, ct);
            var newAds = ads.Where(a => !existingUserUrls.Contains(a.WebpageUrl)).ToList();

            progress?.Report($"Found {ads.Count} listings, evaluating {newAds.Count} new ones...");

            var savedCount = 0;

            foreach (var ad in newAds)
            {
                ct.ThrowIfCancellationRequested();

                progress?.Report($"Scoring: {ad.Headline}");
                var score = await scoringService.MatchesPromptScoreAsync(ToScoringRequest(skillAreas, cvText, ad), ct);
                var posting = JobPostingMapper.FromApi(ad, score);
                posting.UserProfileId = userId;

                if (!posting.IsGoodMatch())
                {
                    logger.LogInformation("Not a good match: {headline}", ad.Headline);
                    progress?.Report($"Skipped: {ad.Headline} (score: {score})");
                    continue;
                }

                writer.Add(posting);
                savedCount += 1;
                logger.LogInformation("Saved job: {headline}", posting.Headline);
                progress?.Report($"Saved: {ad.Headline} (score: {score})");
            }

            return savedCount;
        }

        private static JobScoringCommand ToScoringRequest(ICollection<Domain.Entities.UserSkillArea> skillAreas, string cvText, JobSearchResult ad) => new()
        {
            Headline = ad.Headline,
            Description = ad.Description?.Text ?? string.Empty,
            CvText = cvText,
            UserJobSkills = skillAreas
        };
    }
}