import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private snackBar = inject(MatSnackBar);

  private config: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'right',
    verticalPosition: 'top',
  };

  success(message: string) {
    this.snackBar.open(message, 'Close', {
      ...this.config,
      panelClass: ['bg-green-600', 'text-white'],
    });
  }

  error(message: string) {
    this.snackBar.open(message, 'Close', {
      ...this.config,
      panelClass: ['bg-red-600', 'text-white'],
    });
  }

  info(message: string) {
    this.snackBar.open(message, 'Close', this.config);
  }
}
