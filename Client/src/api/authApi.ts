import { axiosClient } from './axiosClient';
import axios from 'axios';
import { AuthResponse, LoginRequest, RefreshRequest, RevokeRequest } from '../types/auth.types';
import { environment } from '../environments/environment';

export const authApi = {
  login: async (credentials: LoginRequest): Promise<AuthResponse> => {
    const response = await axiosClient.post<AuthResponse>(
      `${environment.apiUrl}/api/auth/login`,
      credentials,
    );
    return response.data;
  },

  refresh: async (accessToken: string): Promise<AuthResponse> => {
    const response = await axiosClient.post<AuthResponse>(`${environment.apiUrl}/api/auth/refresh`);
    return response.data;
  },

  revoke: async (data: RevokeRequest): Promise<void> => {
    await axiosClient.post<void>(`${environment.apiUrl}/api/auth/revoke`, data);
  },
};

export const categoryApi = {
  getCategories: async (): Promise<any> => {
    const response = await axiosClient.get('/api/categories');
    return response.data;
  },
};
