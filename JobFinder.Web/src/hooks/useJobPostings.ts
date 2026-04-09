import { useEffect, useState } from 'react';
import { jobPostingService, type JobPostingResult } from '../services/jobPostingService';

export function useJobPostings() {
  const [postings, setPostings] = useState<JobPostingResult[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    jobPostingService
      .getMyJobPostings()
      .then(setPostings)
      .catch(() => setError('Failed to load job postings'))
      .finally(() => setLoading(false));
  }, []);

  return { postings, loading, error };
}
