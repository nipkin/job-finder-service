const BASE_URL = '/api/auth';

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  id: string;
  userName: string;
}

async function login(request: LoginRequest): Promise<AuthResponse> {
  const response = await fetch(`${BASE_URL}/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message ?? 'Login failed');
  }

  return response.json();
}

async function logout(): Promise<void> {
  await fetch(`${BASE_URL}/logout`, { method: 'POST' });
}

async function register(request: RegisterRequest): Promise<void> {
  const response = await fetch(`${BASE_URL}/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message ?? 'Registration failed');
  }
}

async function check(): Promise<boolean> {
  const response = await fetch(`${BASE_URL}/check`);
  return response.ok;
}

export const authService = { login, logout, register, check };