import { useJobs } from "../hooks/useJobs";

function App() {
    const { jobs, loading } = useJobs();

    if (loading) return <div>Loading...</div>;

    return (
        <div style={{ padding: 20 }}>
            <h1>Job CV Matches</h1>
            <table border={1} cellPadding={10}>
                <thead>
                    <tr>
                        <th>Headline</th>
                        <th>Region</th>
                        <th>Deadline</th>
                        <th>Score</th>
                        <th>Webpage</th>
                        <th>Optimized CV</th>
                    </tr>
                </thead>
                <tbody>
                    {jobs.map((job) => (
                        <tr key={job.id}>
                            <td>{job.headline}</td>
                            <td>{job.region}</td>
                            <td>
                                {job.applicationDeadline
                                    ? new Date(job.applicationDeadline).toLocaleDateString()
                                    : "-"}
                            </td>
                            <td>{job.cvScore}</td>
                            <td>
                                <a href={job.webpageUrl} target="_blank" rel="noopener noreferrer">
                                    Link
                                </a>
                            </td>
                            <td
                                style={{
                                    maxWidth: 300,
                                    overflow: "hidden",
                                    textOverflow: "ellipsis",
                                }}
                            >
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default App;