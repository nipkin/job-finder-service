using System.Text.Json;

namespace JobFinder.Application.JobScoring
{
    public class JobScoringService(IJobScoringClient client) : IJobScoringService
    {
        const int CORE_MAX = 30;
        const int FRONTEND_MAX = 30;
        const int CMS_MAX = 30;
        const int EXPERIENCE_MAX = 5;
        const int SENIORITY_MAX = 5;

        public async Task<int> MatchesPromptScoreAsync(JobScoringRequest ad, CancellationToken ct = default)
        {
            var prompt = BuildScoringPrompt(ad);
            var json = await client.GenerateJsonAsync(prompt, ct);

            var score = JsonSerializer.Deserialize<ScoreBreakdown>(json);
            if (score == null) return 0;

            score = Normalize(score);
            return score.Core + score.Frontend + score.Cms + score.Experience + score.Seniority;
        }

        private static string BuildScoringPrompt(JobScoringRequest ad) => $$"""
            You are a technical recruiter.

            Evaluate how well the job matches the candidate.

            Return ONLY valid JSON.
            No explanations. No markdown. No text outside JSON.

            SCORING RULES (be strict):
            - CANDIDATE CORE SKILLS: 0–30 (backend, architecture, core programming, domain fit)
            - FRONTEND SKILLS: 0–30 (JS frameworks, CSS, frontend architecture)
            - CMS EXPERIENCE: 0–30 (Optimizely, Umbraco, CMS relevance)
            - PREVIOUS EXPERIENCE: 0–5 (project relevance, industry similarity)
            - seniority: 0–5 (seniority and role alignment)

            CANDIDATE CORE SKILLS:
            {{string.Join(", ", ad.UserJobSkills.CoreSkills)}}

            FRONTEND SKILLS:
            {{string.Join(", ", ad.UserJobSkills.FrontendSkills)}}

            CMS EXPERIENCE:
            {{string.Join(", ", ad.UserJobSkills.CmsSkills)}}

            PREVIOUS EXPERIENCE:
            {{ad.UserJobSkills.CvText}}

            JOB TO MATCH, TITLE:
            {{ad.Headline}}

            JOB DESCRIPTION:
            {{ad.Description}}

            The job must be evaluated strictly based on the candidate's skills and experience provided.
            The job must be developer oriented.

            Return JSON exactly in this format:
            {
                "core": int,
                "frontend": int,
                "cms": int,
                "experience": int,
                "seniority": int
            }
        """;

        private static ScoreBreakdown Normalize(ScoreBreakdown s)
        {
            return new ScoreBreakdown
            {
                Core = Math.Clamp(s.Core, 0, CORE_MAX),
                Frontend = Math.Clamp(s.Frontend, 0, FRONTEND_MAX),
                Cms = Math.Clamp(s.Cms, 0, CMS_MAX),
                Experience = Math.Clamp(s.Experience, 0, EXPERIENCE_MAX),
                Seniority = Math.Clamp(s.Seniority, 0, SENIORITY_MAX)
            };
        }   
    }
}