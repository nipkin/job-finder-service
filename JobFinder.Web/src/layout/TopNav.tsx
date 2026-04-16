import { Link } from 'react-router-dom';
import { useAuth, useLogout } from '../hooks';

export default function TopNav() {
  const { isAuthenticated } = useAuth();
  const { logout } = useLogout();

  return (
    <nav className="bg-slate-900 border-b border-slate-800 px-6 py-3 flex items-center justify-between">
      <Link to="/" className="text-indigo-400 font-semibold text-lg tracking-tight hover:text-indigo-300 transition-colors">
        Job Finder
      </Link>
      <ul className="flex items-center gap-5 list-none m-0 p-0">
        {!isAuthenticated && (
          <li><Link to="/" className="text-slate-400 hover:text-white text-sm transition-colors">Home</Link></li>
        )}
        {isAuthenticated && (
          <>
            <li><Link to="/my-page" className="text-slate-400 hover:text-white text-sm transition-colors">My page</Link></li>
            <li><Link to="/my-cv" className="text-slate-400 hover:text-white text-sm transition-colors">My CV</Link></li>
            <li><Link to="/my-skill-areas" className="text-slate-400 hover:text-white text-sm transition-colors">Skills</Link></li>
            <li><Link to="/run-job-match" className="text-slate-400 hover:text-white text-sm transition-colors">Run search</Link></li>
            <li><Link to="/my-job-postings" className="text-slate-400 hover:text-white text-sm transition-colors">Job list</Link></li>
          </>
        )}
        {!isAuthenticated && (
          <>
            <li><Link to="/login" className="text-slate-400 hover:text-white text-sm transition-colors">Login</Link></li>
            <li>
              <Link to="/register" className="bg-indigo-600 hover:bg-indigo-500 text-white text-sm px-3 py-1.5 rounded-lg transition-colors">
                Register
              </Link>
            </li>
          </>
        )}
        {isAuthenticated && (
          <li>
            <button
              onClick={logout}
              className="text-slate-400 hover:text-white text-sm transition-colors cursor-pointer bg-transparent border-0 p-0"
            >
              Logout
            </button>
          </li>
        )}
      </ul>
    </nav>
  );
}
