import { useEffect, useState } from 'react';
import { authService } from '../services/authService';
import { AuthContext } from './AuthContext';

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [checking, setChecking] = useState(true);

  useEffect(() => {
    authService.check()
      .then(setIsAuthenticated)
      .finally(() => setChecking(false));
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated, checking }}>
      {children}
    </AuthContext.Provider>
  );
}
