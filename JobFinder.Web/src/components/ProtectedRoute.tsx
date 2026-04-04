import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../hooks';

export default function ProtectedRoute() {
  const { isAuthenticated, checking } = useAuth();

  if (checking) return null;
  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
}
