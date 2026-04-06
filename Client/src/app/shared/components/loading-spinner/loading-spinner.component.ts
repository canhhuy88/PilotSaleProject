import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule],
  template: `
    <div *ngIf="loading" class="flex justify-center items-center p-4">
      <mat-spinner [diameter]="diameter"></mat-spinner>
    </div>
  `,
})
export class LoadingSpinnerComponent {
  @Input() loading = false;
  @Input() diameter = 40;
}
