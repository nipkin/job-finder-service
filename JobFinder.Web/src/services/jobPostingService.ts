export interface JobPostingResult {
  id: string;
  headline: string;
  region: string | null;
  description: string;
  applicationDeadline: string | null;
  webpageUrl: string;
  createdAtUtc: string;
  optimizedCv: string | null;
  cvScore: number | null;
}

async function getMyJobPostings(): Promise<JobPostingResult[]> {
  const response = await fetch('/api/jobposting/my');
  if (!response.ok) throw new Error('Failed to fetch job postings');
  return response.json();
}

export const jobPostingService = { getMyJobPostings };
