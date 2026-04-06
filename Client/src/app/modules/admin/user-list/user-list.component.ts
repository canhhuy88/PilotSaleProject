import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import {
  DataTableComponent,
  TableColumn,
} from '../../../shared/components/data-table/data-table.component';
import { UserService } from '../../../core/services/user.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { User } from '../../../shared/models/user.model';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, MatDialogModule, DataTableComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-2xl font-bold text-gray-800">Staff Management</h1>
          <p class="text-gray-500">Manage user access and roles</p>
        </div>
        <button
          mat-flat-button
          color="primary"
          (click)="addUser()"
          class="rounded-lg h-11 px-6 shadow-md shadow-primary-100"
        >
          <mat-icon class="mr-2">person_add</mat-icon>
          Add Staff Member
        </button>
      </div>

      <app-data-table
        [columns]="columns"
        [data]="(users$ | async) || []"
        (onEdit)="editUser($event)"
        (onDelete)="deleteUser($event)"
      ></app-data-table>
    </div>
  `,
})
export class UserListComponent {
  private userService = inject(UserService);
  private notification = inject(NotificationService);
  private dialog = inject(MatDialog);

  users$ = this.userService.getUsers();

  columns: TableColumn[] = [
    { key: 'username', label: 'Username' },
    { key: 'fullName', label: 'Full Name' },
    { key: 'role', label: 'Role' },
    { key: 'actions', label: 'Actions', type: 'actions' },
  ];

  addUser() {
    this.notification.info('Add user feature coming soon');
  }

  editUser(user: User) {
    this.notification.info('Edit user feature coming soon');
  }

  deleteUser(user: User) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Staff Member',
        message: `Are you sure you want to remove access for ${user.fullName}?`,
        primaryColor: 'warn',
        primaryText: 'Remove Access',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.userService.deleteUser(user.id).subscribe(() => {
          this.notification.success('User removed successfully');
          this.users$ = this.userService.getUsers();
        });
      }
    });
  }
}
