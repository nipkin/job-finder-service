import { useJobPostings } from '../hooks';

export default function MyJobPostings() {
  const { postings, loading, error } = useJobPostings();

  if (loading) return <p className="text-slate-400">Loading...</p>;
  if (error) return <p className="text-red-400">{error}</p>;

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-3xl font-bold text-white">My Job Postings</h1>
        <p className="text-slate-400 mt-1">{postings.length} matched posting{postings.length !== 1 ? 's' : ''}</p>
      </div>

      {postings.length === 0 ? (
        <p className="text-slate-500">No job postings found. Run a job match to find results.</p>
      ) : (
        <div className="flex flex-col gap-4">
          {postings.map(p => {
            const score = p.cvScore != null ? Math.round(p.cvScore) : null;
            const scoreColor = score == null ? '' : score >= 70 ? 'text-emerald-400' : 'text-amber-400';

            return (
              <div key={p.id} className="bg-slate-900 border border-slate-800 rounded-xl p-5 flex flex-col gap-3">
                <div className="flex justify-between items-start gap-4">
                  <h2 className="text-white font-semibold text-base leading-snug">{p.headline}</h2>
                  {score != null && (
                    <span className={`text-sm font-bold shrink-0 ${scoreColor}`}>{score}%</span>
                  )}
                </div>

                <div className="flex gap-4 text-xs text-slate-500">
                  {p.region && <span>{p.region}</span>}
                  {p.applicationDeadline && (
                    <span>Deadline: {new Date(p.applicationDeadline).toLocaleDateString()}</span>
                  )}
                </div>

                <p className="text-slate-400 text-sm whitespace-pre-wrap leading-relaxed">
                  {p.description.length > 300 ? p.description.slice(0, 300) + '…' : p.description}
                </p>

                <div className="flex gap-4 text-sm items-start">
                  {p.webpageUrl && (
                    <a
                      href={p.webpageUrl}
                      target="_blank"
                      rel="noreferrer"
                      className="text-indigo-400 hover:text-indigo-300 transition-colors"
                    >
                      View posting
                    </a>
                  )}
                  {p.optimizedCv && (
                    <details className="flex-1">
                      <summary className="text-slate-400 hover:text-white cursor-pointer transition-colors">
                        Optimized CV
                      </summary>
                      <pre className="mt-2 bg-slate-800 border border-slate-700 rounded-lg p-3 text-slate-300 text-xs whitespace-pre-wrap leading-relaxed">
                        {p.optimizedCv}
                      </pre>
                    </details>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}
