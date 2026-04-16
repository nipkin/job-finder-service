import { Link } from 'react-router-dom';

const links = [
  { to: '/my-cv', label: 'My CV', description: 'Paste your CV. Used by the AI when matching job postings.' },
  { to: '/my-skill-areas', label: 'My skill areas', description: 'Add skills and set their importance weight (1–5).' },
  { to: '/run-job-match', label: 'Run search', description: 'Start an automated job search and AI matching process.' },
  { to: '/my-job-postings', label: 'Job list', description: 'Browse matched job postings with AI-scored results.' },
];

export default function MyPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-3xl font-bold text-white">Welcome back</h1>
        <p className="text-slate-400 mt-1">Your AI-powered job search dashboard.</p>
      </div>
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        {links.map(({ to, label, description }) => (
          <Link
            key={to}
            to={to}
            className="bg-slate-900 border border-slate-800 hover:border-indigo-500 rounded-xl p-5 flex flex-col gap-1.5 transition-colors group"
          >
            <span className="text-white font-semibold group-hover:text-indigo-400 transition-colors">{label}</span>
            <span className="text-slate-400 text-sm">{description}</span>
          </Link>
        ))}
      </div>
    </div>
  );
}
