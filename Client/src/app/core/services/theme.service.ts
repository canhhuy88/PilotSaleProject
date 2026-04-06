import { Injectable, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

export type Theme = 'light' | 'dark';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private document = inject(DOCUMENT);
  private readonly THEME_KEY = 'app-theme';

  private themeSubject = new BehaviorSubject<Theme>(this.getInitialTheme());
  theme$ = this.themeSubject.asObservable();

  constructor() {
    this.applyTheme(this.themeSubject.value);
    this.listenToSystemChanges();
  }

  get currentTheme(): Theme {
    return this.themeSubject.value;
  }

  setTheme(theme: Theme) {
    this.themeSubject.next(theme);
    this.applyTheme(theme);
    localStorage.setItem(this.THEME_KEY, theme);
  }

  toggleTheme() {
    const newTheme = this.currentTheme === 'light' ? 'dark' : 'light';
    this.setTheme(newTheme);
  }

  private getInitialTheme(): Theme {
    const savedTheme = localStorage.getItem(this.THEME_KEY) as Theme;
    if (savedTheme && (savedTheme === 'light' || savedTheme === 'dark')) {
      return savedTheme;
    }
    return this.getSystemPreference();
  }

  private getSystemPreference(): Theme {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    return 'light';
  }

  private applyTheme(theme: Theme) {
    const html = this.document.documentElement;
    html.classList.remove('theme-light', 'theme-dark', 'light', 'dark');
    html.classList.add(`theme-${theme}`);

    // Support Tailwind dark class
    if (theme === 'dark') {
      html.classList.add('dark');
    }
  }

  private listenToSystemChanges() {
    if (window.matchMedia) {
      // Listen to OS preference changes
      window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
        if (!localStorage.getItem(this.THEME_KEY)) {
          this.setTheme(e.matches ? 'dark' : 'light');
        }
      });

      // Sync theme across tabs
      window.addEventListener('storage', (event) => {
        if (event.key === this.THEME_KEY && event.newValue) {
          const newTheme = event.newValue as Theme;
          if (newTheme === 'light' || newTheme === 'dark') {
            this.themeSubject.next(newTheme);
            this.applyTheme(newTheme);
          }
        }
      });
    }
  }
}
