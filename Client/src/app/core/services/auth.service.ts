import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  getUser,
  setAuthData,
  clearAuthData,
  isAuthenticated,
  getAccessToken,
  getRefreshToken,
  hasRole,
} from '../../../utils/tokenStorage';
import { AuthResponse, LoginRequest } from '../../../types/auth.types';
import { authApi } from '../../../api/authApi';

// A hack for non-Angular code (like axiosClient.ts) to reach the singleton
export let currentAuthServiceInstance: AuthService | null = null;

export const authService = {
  login: (credentials: LoginRequest) => currentAuthServiceInstance!.login(credentials),
  refreshToken: () => currentAuthServiceInstance!.refreshToken(),
  logout: () => currentAuthServiceInstance!.logout(),
};

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = environment.apiUrl;

  private currentUserSubject = new BehaviorSubject<any>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    currentAuthServiceInstance = this;

    // Migrate legacy logic to use tokenStorage
    let user = getUser();

    // Legacy fallback
    if (!user) {
      const userStr = localStorage.getItem('user');
      if (userStr) {
        try {
          user = JSON.parse(userStr);
        } catch (e) {}
      }
    }

    if (user) {
      this.currentUserSubject.next(user);
    }
  }

  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const data = await authApi.login(credentials);
    if (!data.accessToken) {
      throw new Error('Login failed: No data returned');
    }

    const authData: AuthResponse = {
      accessToken: data.accessToken,
      refreshToken: data.refreshToken,
      username: data.username,
      role: data.role,
    };

    setAuthData(authData);
    this.currentUserSubject.next(getUser() || data);
    return data;
  }

  async refreshToken(): Promise<AuthResponse> {
    const accessToken = getAccessToken();
    //const refreshToken = getRefreshToken();

    if (!accessToken /* || !refreshToken */) {
      this.logoutSync();
      throw new Error('Missing tokens');
    }

    try {
      const data = await authApi.refresh(accessToken);
      setAuthData({
        accessToken: data.accessToken,
        refreshToken: data.refreshToken,
        username: data.username,
        role: data.role,
      });
      this.currentUserSubject.next(getUser());
      return data;
    } catch (error) {
      this.logoutSync();
      throw error;
    }

    return { accessToken: '' } as AuthResponse; // Placeholder since refresh logic is disabled
  }

  async logout(): Promise<void> {
    const refreshToken = getRefreshToken();
    if (refreshToken) {
      try {
        await authApi.revoke({ refreshToken } as any);
      } catch (error) {
        console.error('Failed to revoke token', error);
      }
    }

    this.logoutSync();
  }

  private logoutSync() {
    clearAuthData();
    localStorage.removeItem('token'); // clean legacy keys
    localStorage.removeItem('user'); // clean legacy keys
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  isLoggedIn() {
    return this.isAuthenticated();
  }

  isAuthenticated(): boolean {
    if (isAuthenticated()) {
      return true;
    }
    // Legacy fallback
    return !!localStorage.getItem('token');
  }

  getUser(): any {
    return getUser();
  }

  hasRole(role: string): boolean {
    return hasRole(role);
  }
}
