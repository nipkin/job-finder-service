import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import { useAuth } from './useAuth';

export function useLogout() {
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();

  async function logout() {
    await authService.logout();
    setIsAuthenticated(false);
    navigate('/login');
  }

  return { logout };
}