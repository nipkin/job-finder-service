export interface JobResult {
    id: number;
    headline: string;
    region: string;
    description: string;
    applicationDeadline?: string;
    webpageUrl: string;
    createdAtUtc: string;
    cvScore: number;
    optimizedCv: string;
}