export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  username: string;
  role: string;
}

export interface LoginRequest {
  username: string;
  password?: string;
}

export interface RefreshRequest {
  //refreshToken: string;
}

export interface RevokeRequest {
  refreshToken: string;
}
