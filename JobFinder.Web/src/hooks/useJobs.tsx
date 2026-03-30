import { useEffect, useState } from "react";
import type { JobResult } from "../types/jobResult";

export function useJobs() {
    const [jobs, setJobs] = useState<JobResult[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchJobs = async () => {
            try {
                const res = await fetch("/api/JobPosting");
                if (!res.ok) throw new Error("Network response was not ok");
                const data: JobResult[] = await res.json();
                const sortedData = data.sort(
                    (a, b) => b.cvScore - a.cvScore
                );

                setJobs(sortedData);
            } catch (err) {
                console.error("Error fetching job CVs:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchJobs();
    }, []);

    return {
        jobs, setJobs,
        loading, setLoading
    };
}