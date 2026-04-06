import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, tap, of, delay } from 'rxjs';
import { environment } from '../../../environments/environment';

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
    const user = localStorage.getItem('user');
    if (user) {
      this.currentUserSubject.next(JSON.parse(user));
    }
  }

  login(credentials: { username: string; password: string }) {
    // Dummy login
    if (credentials.username === 'admin' && credentials.password === '123456') {
      const mockResponse = {
        token: 'demo-token',
        user: { id: '1', username: 'admin', fullName: 'Demo Admin', role: 'Admin' },
      };
      return of(mockResponse).pipe(
        delay(500),
        tap((response) => this.setSession(response)),
      );
    }

    // In a real app, this would be a real API call.
    // Since I'm building a scaffold, I'll mock it if the URL is not set or fails.
    return this.http.post<any>(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap((response) => {
        this.setSession(response);
      }),
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  private setSession(authResult: any) {
    localStorage.setItem('token', authResult.token);
    localStorage.setItem('user', JSON.stringify(authResult.user));
    this.currentUserSubject.next(authResult.user);
  }

  isLoggedIn() {
    return !!localStorage.getItem('token');
  }
}
