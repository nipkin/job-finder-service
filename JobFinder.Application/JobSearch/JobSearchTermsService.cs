using JobFinder.Application.Common;
using JobFinder.Application.JobScoring;
using JobFinder.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace JobFinder.Application.JobSearch
{
    public class JobSearchTermsService(IJobScoringClient scoringClient) : IJobSearchTermsService
    {
        public async Task<Result<List<string>>> GenerateSearchTermsAsync(ICollection<UserSkillArea> userSkillAreas, CancellationToken ct = default)
        {
            if (userSkillAreas.Count == 0)
                return Result<List<string>>.Fail(new Error("User has no skill areas defined", 400));

            var prompt = BuildPrompt(userSkillAreas);
            var json = await scoringClient.GenerateJsonAsync(prompt, ct);

            var searchTerms = ParseSearchTerms(json);
            if (searchTerms == null)
                return Result<List<string>>.Fail(new Error("Failed to parse search terms from LLM response", 500));

            return Result<List<string>>.Ok(searchTerms);
        }

        private static List<string>? ParseSearchTerms(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("searchTerms", out var termsElement))
                {
                    return termsElement
                        .EnumerateArray()
                        .Select(t => t.GetString())
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .Select(t => t!)
                        .ToList();
                }

                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static string BuildPrompt(IEnumerable<UserSkillArea> skillAreas)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a job search assistant. Generate a list of concise job search terms based on the following skill areas and skills.");
            sb.AppendLine("Return ONLY a JSON object in this exact format: { \"searchTerms\": [\"term1\", \"term2\", ...] }");
            sb.AppendLine();
            sb.AppendLine("Each skill area has a weight from 1-10 indicating its importance to the user.");
            sb.AppendLine("Rules:");
            sb.AppendLine("- Higher weight areas should contribute MORE search terms and have greater influence on the results.");
            sb.AppendLine("- Lower weight areas should contribute FEWER search terms, but still be represented.");
            sb.AppendLine("- Combine skills across areas where it makes sense (e.g. 'Senior React Developer', 'Full Stack Engineer').");
            sb.AppendLine("- Terms should be realistic job titles or search queries used on LinkedIn or Indeed.");
            sb.AppendLine();
            sb.AppendLine("Skill areas (sorted by importance, highest first):");

            var sorted = skillAreas.OrderByDescending(a => a.SkillWeight).ToList();
            int totalWeight = sorted.Sum(a => a.SkillWeight);

            foreach (var area in sorted)
            {
                var skillNames = area.UserSkills.Select(s => s.Name).ToList();
                int suggestedTerms = (int)Math.Round((double)area.SkillWeight / totalWeight * 12);
                suggestedTerms = Math.Max(1, suggestedTerms); // always at least 1 term per area

                sb.AppendLine($"- [{area.Name}] Weight: {area.SkillWeight}/10 → suggest ~{suggestedTerms} terms");
                sb.AppendLine($"  Skills: {string.Join(", ", skillNames)}");
            }

            sb.AppendLine();
            sb.AppendLine("Generate 10-14 search terms total, distributed roughly as suggested above.");

            return sb.ToString();
        }
    }
}
