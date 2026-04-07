import { axiosClient } from './axiosClient';
import axios from 'axios';
import { AuthResponse, LoginRequest, RefreshRequest, RevokeRequest } from '../types/auth.types';

export const authApi = {
  login: async (credentials: LoginRequest): Promise<AuthResponse> => {
    const response = await axiosClient.post<AuthResponse>('/api/auth/login', credentials);
    return response.data;
  },

  refresh: async (data: RefreshRequest, accessToken: string): Promise<AuthResponse> => {
    const response = await axiosClient.post<AuthResponse>('/api/auth/refresh', data);
    return response.data;
  },

  revoke: async (data: RevokeRequest): Promise<void> => {
    await axiosClient.post<AuthResponse>('/api/auth/revoke', data);
  },
};
