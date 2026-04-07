using JobFinder.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace JobFinder.Application.JobScoring
{
    public class JobScoringService(IJobScoringClient client) : IJobScoringService
    {
        public async Task<int> MatchesPromptScoreAsync(JobScoringCommand request, CancellationToken ct = default)
        {
            var skillAreas = request.UserJobSkills.ToList();
            var prompt = BuildScoringPrompt(skillAreas, request.CvText, request.Headline, request.Description);
            var json = await client.GenerateJsonAsync(prompt, ct);

            var scores = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            if (scores is null) return 0;

            return (int)Math.Round(Normalize(scores, skillAreas));
        }

        private static string BuildScoringPrompt(List<UserSkillArea> skillAreas, string cvText, string adHeadline, string adDescription)
        {
            var rules = new StringBuilder();
            rules.AppendLine("SCORING RULES (be strict):");
            for (int i = 0; i < skillAreas.Count; i++)
            {
                var area = skillAreas[i];
                rules.AppendLine($"- s{i}: 0-{area.SkillWeight} ({area.Name}: {string.Join(", ", area.UserSkills.Select(s => s.Name))})");
            }

            var jsonFormat = new StringBuilder();
            jsonFormat.AppendLine("{");
            for (int i = 0; i < skillAreas.Count; i++)
            {
                var comma = i < skillAreas.Count - 1 ? "," : "";
                jsonFormat.AppendLine($"    \"s{i}\": int{comma}");
            }
            jsonFormat.Append("}");

            return $$"""
                You are a technical recruiter.

                Evaluate how well the job matches the candidate.

                Return ONLY valid JSON.
                No explanations. No markdown. No text outside JSON.

                {{rules}}
                CANDIDATE SKILLS:
                {{cvText}}

                JOB TITLE:
                {{adHeadline}}

                JOB DESCRIPTION:
                {{adDescription}}

                The job must be evaluated strictly based on the candidate's skills and experience provided.
                The job must be developer oriented.

                Return JSON exactly in this format:
                {{jsonFormat}}
            """;
        }

        private static double Normalize(Dictionary<string, int> scores, List<UserSkillArea> skillAreas)
        {
            var maxTotal = skillAreas.Sum(a => a.SkillWeight);
            if (maxTotal == 0) return 0;

            var actualTotal = skillAreas.Select((area, i) =>
            {
                var key = $"s{i}";
                return scores.TryGetValue(key, out var score)
                    ? Math.Clamp(score, 0, area.SkillWeight)
                    : 0;
            }).Sum();

            return (double)actualTotal / maxTotal * 100;
        }
    }
}