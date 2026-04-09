import { useJobPostings } from '../hooks';

export default function MyJobPostings() {
  const { postings, loading, error } = useJobPostings();

  if (loading) return <p>Loading...</p>;
  if (error) return <p style={{ color: 'red' }}>{error}</p>;

  return (
    <div style={{ maxWidth: 800, margin: '0 auto', padding: '1rem' }}>
      <h1>My Job Postings</h1>

      {postings.length === 0 ? (
        <p style={{ opacity: 0.6 }}>No job postings found. Run a job match to find results.</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          {postings.map(p => (
            <div
              key={p.id}
              style={{
                border: '1px solid #444',
                borderRadius: 8,
                padding: '1rem',
              }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', gap: '1rem' }}>
                <h2 style={{ margin: 0, fontSize: '1.1em' }}>{p.headline}</h2>
                {p.cvScore != null && (
                  <span
                    style={{
                      flexShrink: 0,
                      fontWeight: 'bold',
                      color: p.cvScore >= 70 ? '#4caf50' : '#ff9800',
                    }}
                  >
                    {Math.round(p.cvScore)}%
                  </span>
                )}
              </div>

              {p.region && (
                <p style={{ margin: '0.25rem 0 0', opacity: 0.6, fontSize: '0.9em' }}>{p.region}</p>
              )}

              {p.applicationDeadline && (
                <p style={{ margin: '0.25rem 0 0', fontSize: '0.85em', opacity: 0.7 }}>
                  Deadline: {new Date(p.applicationDeadline).toLocaleDateString()}
                </p>
              )}

              <p style={{ margin: '0.75rem 0 0', fontSize: '0.9em', whiteSpace: 'pre-wrap' }}>
                {p.description.length > 300 ? p.description.slice(0, 300) + '…' : p.description}
              </p>

              <div style={{ marginTop: '0.75rem', display: 'flex', gap: '1rem', fontSize: '0.85em' }}>
                {p.webpageUrl && (
                  <a href={p.webpageUrl} target="_blank" rel="noreferrer">
                    View posting
                  </a>
                )}
                {p.optimizedCv && (
                  <details>
                    <summary style={{ cursor: 'pointer' }}>Optimized CV</summary>
                    <pre
                      style={{
                        marginTop: '0.5rem',
                        background: '#1a1a1a',
                        padding: '0.75rem',
                        borderRadius: 4,
                        whiteSpace: 'pre-wrap',
                        fontSize: '0.85em',
                      }}
                    >
                      {p.optimizedCv}
                    </pre>
                  </details>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
