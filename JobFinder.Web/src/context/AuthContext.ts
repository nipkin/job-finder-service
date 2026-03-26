import { createContext } from 'react';

export interface AuthContextType {
  isAuthenticated: boolean;
  setIsAuthenticated: (val: boolean) => void;
}

export const AuthContext = createContext<AuthContextType | null>(null);
