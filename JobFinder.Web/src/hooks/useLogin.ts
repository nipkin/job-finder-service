import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';
import { useAuth } from '../hooks';

export function useLogin() {
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);

    try {
      await authService.login({ userName, password });
      setIsAuthenticated(true);
      navigate('/my-page');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Login failed');
    }
  }

  return {
    userName, setUserName,
    password, setPassword,
    error,
    handleSubmit,
  };
}
