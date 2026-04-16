import { Link } from 'react-router-dom';
import { useAuth, useLogout } from '../hooks';

export default function TopNav() {
  const { isAuthenticated } = useAuth();
  const { logout } = useLogout();

  return (
    <nav>
      <Link to="/">Job Finder</Link>
      <ul>
        {!isAuthenticated && <li><Link to="/">Home</Link></li>}
        {isAuthenticated && <li><Link to="/my-page">My page</Link></li>}
        {isAuthenticated && <li><Link to="/my-cv">My CV</Link></li>}
        {isAuthenticated && <li><Link to="/my-skill-areas">My skill areas</Link></li>}
        {isAuthenticated && <li><Link to="/run-job-match">Run search</Link></li>}
        {isAuthenticated && <li><Link to="/my-job-postings">My joblist</Link></li>}
        {!isAuthenticated && <li><Link to="/login">Login</Link></li>}
        {!isAuthenticated && <li><Link to="/register">Register</Link></li>}
        {isAuthenticated && <li><button onClick={logout}>Logout</button></li>}
      </ul>
    </nav>
  );
}