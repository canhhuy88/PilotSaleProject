import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import { getAccessToken } from '../utils/tokenStorage';
import { mapApiError } from '../utils/errorUtils';
import { environment } from '../environments/environment';
import { showErrorToast } from '../utils/toastBridge';

const BASE_URL = environment.apiUrl;

export const axiosClient = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Refresh token queue state
let isRefreshing = false;
let failedQueue: Array<{
  resolve: (value?: unknown) => void;
  reject: (reason?: any) => void;
}> = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

axiosClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = getAccessToken();
    // Do not attach token for login endpoint
    if (token && config.url !== '/api/auth/login') {
      config.headers.Authorization = `${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

axiosClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const status = error.response?.status;
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

    if ((status === 401 || status === 400) && originalRequest && !originalRequest._retry) {
      if (isRefreshing) {
        try {
          // Wait for the ongoing refresh request to complete, then retry with new token
          await new Promise((resolve, reject) => {
            failedQueue.push({ resolve, reject });
          });
          return axiosClient(originalRequest);
        } catch (err: any) {
          const mapped = mapApiError(err);
          showErrorToast(mapped.errorMessage);
          return Promise.reject({
            errorMessage: mapped.errorMessage,
            status: err?.response?.status,
          });
        }
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const { authService } = await import('../app/core/services/auth.service');
        const newAuthData = await authService.refreshToken();

        processQueue(null, newAuthData.accessToken);

        // Update authorization header and retry original failed request
        originalRequest.headers.Authorization = `Bearer ${newAuthData.accessToken}`;
        return axiosClient(originalRequest);
      } catch (refreshError: any) {
        processQueue(refreshError, null);
        const { clearAuthData } = await import('../utils/tokenStorage');
        clearAuthData();
        // window.location.href = '/auth/login';

        const mapped = mapApiError(refreshError);
        showErrorToast(mapped.errorMessage);
        return Promise.reject({
          errorMessage: mapped.errorMessage,
          status: refreshError?.response?.status,
        });
      } finally {
        isRefreshing = false;
      }
    }

    const mapped = mapApiError(error);
    showErrorToast(mapped.errorMessage);
    return Promise.reject({
      errorMessage: mapped.errorMessage,
      status: status,
    });
  },
);
