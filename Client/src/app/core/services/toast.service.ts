import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface ToastMessage {
  id: string;
  type: 'success' | 'error' | 'info';
  message: string;
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private toastSubject = new Subject<ToastMessage>();
  toastState$ = this.toastSubject.asObservable();

  showSuccess(message: string) {
    this.show('success', message);
  }

  showError(message: string) {
    this.show('error', message);
  }

  showInfo(message: string) {
    this.show('info', message);
  }

  private show(type: 'success' | 'error' | 'info', message: string) {
    const id = Math.random().toString(36).substring(2, 9);
    this.toastSubject.next({ id, type, message });
  }
}
