import { Component, signal, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './core/services/theme.service';
import { ToastComponent } from './shared/components/toast/toast.component';
import { ToastService } from './core/services/toast.service';
import { setToastHandler } from '../utils/toastBridge';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ToastComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  protected readonly title = signal('BizI.Client');
  private themeService = inject(ThemeService); // Initialize theme on app load
  private toastService = inject(ToastService);

  ngOnInit(): void {
    setToastHandler((message: string) => {
      this.toastService.showError(message);
    });
  }
}
