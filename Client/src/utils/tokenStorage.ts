import { AuthResponse } from '../types/auth.types';

const AUTH_KEY = 'auth_data';

export const getAuthData = (): AuthResponse | null => {
  const data = localStorage.getItem(AUTH_KEY);
  return data ? JSON.parse(data) : null;
};

export const getAccessToken = (): string | null => {
  return getAuthData()?.accessToken || null;
};

export const getRefreshToken = (): string | null => {
  return getAuthData()?.refreshToken || null;
};

export const getUser = () => {
  const data = getAuthData();
  if (!data) return null;
  return { username: data.username, role: data.role };
};

export const setAuthData = (data: AuthResponse): void => {
  localStorage.setItem(AUTH_KEY, JSON.stringify(data));
};

export const clearAuthData = (): void => {
  localStorage.removeItem(AUTH_KEY);
};

// Bonus: isAuthenticated helper with JWT expiry check
export const isAuthenticated = (): boolean => {
  const token = getAccessToken();
  if (!token) return false;

  try {
    const payloadStart = token.indexOf('.') + 1;
    const payloadEnd = token.lastIndexOf('.');
    if (payloadStart > 0 && payloadEnd > payloadStart) {
      const payload = token.substring(payloadStart, payloadEnd);
      // Basic base64 decoding for JWT
      const decodedData = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
      const decoded = JSON.parse(decodedData);

      if (decoded.exp) {
        const isExpired = decoded.exp * 1000 < Date.now();
        if (isExpired) return false; // Token has expired
      }
    }
  } catch (e) {
    // Ignore parsing errors and fallback to token existence
  }
  return true;
};

// Bonus: role-based helper
export const hasRole = (role: string): boolean => {
  const user = getUser();
  return user?.role === role;
};
