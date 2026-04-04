import { createContext } from 'react';

export interface AuthContextType {
  isAuthenticated: boolean;
  setIsAuthenticated: (val: boolean) => void;
  checking: boolean;
}

export const AuthContext = createContext<AuthContextType | null>(null);
