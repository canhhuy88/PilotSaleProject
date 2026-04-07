import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, ToastMessage } from '../../../core/services/toast.service';
import { animate, style, transition, trigger } from '@angular/animations';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.scss',
  animations: [
    trigger('toastAnimations', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('300ms ease-in-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
      transition(':leave', [
        animate('300ms ease-in-out', style({ opacity: 0, transform: 'translateY(-20px)' })),
      ]),
    ]),
  ],
})
export class ToastComponent implements OnInit, OnDestroy {
  toasts: ToastMessage[] = [];
  private toastService = inject(ToastService);
  private subscription: Subscription = new Subscription();
  private maxToasts = 3;

  ngOnInit() {
    this.subscription = this.toastService.toastState$.subscribe((toast) => {
      // Prevent duplicate consecutive messages
      if (this.toasts.length > 0 && this.toasts[this.toasts.length - 1].message === toast.message) {
        return;
      }

      this.toasts.push(toast);

      if (this.toasts.length > this.maxToasts) {
        this.toasts.shift();
      }

      setTimeout(() => this.remove(toast.id), 5000);
    });
  }

  remove(id: string) {
    this.toasts = this.toasts.filter((t) => t.id !== id);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
