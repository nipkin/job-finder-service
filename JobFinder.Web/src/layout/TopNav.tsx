import { Link } from 'react-router-dom';

export default function TopNav() {
  return (
    <nav>
      <Link to="/">Job Finder</Link>
      <ul>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/my-page">My Page</Link></li>
        <li><Link to="/login">Login</Link></li>
        <li><Link to="/register">Register</Link></li>
      </ul>
    </nav>
  );
}