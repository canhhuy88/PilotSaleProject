import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data.title || 'Confirm Action' }}</h2>
    <mat-dialog-content>
      <p>{{ data.message || 'Are you sure you want to proceed?' }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onSecondaryClick()">{{ data.secondaryText || 'Cancel' }}</button>
      <button mat-raised-button color="primary" (click)="onPrimaryClick()">
        {{ data.primaryText || 'Confirm' }}
      </button>
    </mat-dialog-actions>
  `,
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {}

  onPrimaryClick(): void {
    this.dialogRef.close(true);
  }

  onSecondaryClick(): void {
    this.dialogRef.close(false);
  }
}
