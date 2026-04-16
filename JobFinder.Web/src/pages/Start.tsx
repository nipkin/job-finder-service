import { Link } from 'react-router-dom';
import { useAuth } from '../hooks';

export default function Start() {
  const { isAuthenticated } = useAuth();

  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh] text-center gap-6">
      <h1 className="text-5xl font-bold text-white">Find your next job,<br />
        <span className="text-indigo-400">powered by AI</span>
      </h1>
      <p className="text-slate-400 text-lg max-w-lg">
        Automate your job search and let an AI assistant match postings against your skills and CV.
      </p>
      <div className="flex gap-3 mt-2">
        <Link
          to={isAuthenticated ? '/my-page' : '/'}
          className="bg-indigo-600 hover:bg-indigo-500 text-white px-6 py-2.5 rounded-lg font-medium transition-colors"
        >
          Get started
        </Link>
         {!isAuthenticated &&
             <Link
                to="/login"
                className="bg-slate-800 hover:bg-slate-700 text-slate-200 px-6 py-2.5 rounded-lg font-medium transition-colors border border-slate-700"
                >
                Log in
             </Link>
            }
      </div>
    </div>
  );
}
