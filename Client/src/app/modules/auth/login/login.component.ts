import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../shared/services/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  template: `
    <div class="min-h-screen flex items-center justify-center bg-slate-50 p-4">
      <div class="w-full max-w-md">
        <div class="text-center mb-8">
          <div
            class="inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-primary-600 text-white mb-4 shadow-lg shadow-primary-200"
          >
            <mat-icon class="scale-150">shopping_cart</mat-icon>
          </div>
          <h1 class="text-3xl font-bold text-slate-900">BizI POS</h1>
          <p class="text-slate-500 mt-2">Manage your sales efficiently</p>
        </div>

        <form
          [formGroup]="loginForm"
          (ngSubmit)="onSubmit()"
          class="bg-white p-8 rounded-2xl shadow-xl shadow-slate-200 border border-slate-100"
        >
          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-slate-700 mb-2">Username</label>
              <input
                type="text"
                formControlName="username"
                class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-primary-500 focus:ring-4 focus:ring-primary-50 outline-none transition-all"
                placeholder="Enter your username"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-slate-700 mb-2">Password</label>
              <input
                type="password"
                formControlName="password"
                class="w-full px-4 py-3 rounded-xl border border-slate-200 focus:border-primary-500 focus:ring-4 focus:ring-primary-50 outline-none transition-all"
                placeholder="••••••••"
              />
            </div>

            <div class="flex items-center justify-between">
              <label class="flex items-center">
                <input
                  type="checkbox"
                  class="rounded border-slate-300 text-primary-600 shadow-sm focus:border-primary-300 focus:ring focus:ring-primary-200 focus:ring-opacity-50"
                />
                <span class="ml-2 text-sm text-slate-600">Remember me</span>
              </label>
              <a href="#" class="text-sm font-medium text-primary-600 hover:text-primary-500"
                >Forgot password?</a
              >
            </div>

            <button
              type="submit"
              [disabled]="loading"
              class="w-full bg-primary-600 hover:bg-primary-700 text-white font-bold py-3.5 rounded-xl transition-all shadow-lg shadow-primary-100 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {{ loading ? 'Signing in...' : 'Sign In' }}
            </button>

            <div class="text-center">
              <p class="text-slate-500 text-sm">Demo: admin / 123456</p>
            </div>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private notification = inject(NotificationService);

  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });

  loading = false;

  onSubmit() {
    if (this.loginForm.valid) {
      this.loading = true;
      const { username, password } = this.loginForm.value;

      this.authService.login({ username: username!, password: password! }).subscribe({
        next: () => {
          this.notification.success('Welcome back!');
          this.router.navigate(['/pos']);
        },
        error: (err) => {
          this.loading = false;
          this.notification.error('Login failed! Please check your credentials.');
          console.error(err);
        },
      });
    }
  }
}
