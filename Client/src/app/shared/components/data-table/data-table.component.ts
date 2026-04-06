import { Component, Input, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface TableColumn {
  key: string;
  label: string;
  type?: 'text' | 'number' | 'currency' | 'date' | 'actions';
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
  ],
  template: `
    <div class="overflow-auto rounded-lg shadow-sm border pos-card">
      <table
        mat-table
        [dataSource]="dataSource"
        matSort
        class="w-full !bg-surface !text-text-primary transition-theme"
      >
        <ng-container *ngFor="let col of columns" [matColumnDef]="col.key">
          <th
            mat-header-cell
            *matHeaderCellDef
            mat-sort-header
            class="!bg-background !text-text-secondary !border-b !border-border transition-theme"
          >
            {{ col.label }}
          </th>
          <td
            mat-cell
            *matCellDef="let element"
            class="!text-text-primary !border-b !border-border transition-theme"
          >
            <ng-container [ngSwitch]="col.type">
              <span *ngSwitchCase="'currency'">{{ element[col.key] | currency }}</span>
              <span *ngSwitchCase="'date'">{{ element[col.key] | date }}</span>
              <div *ngSwitchCase="'actions'" class="flex gap-2">
                <button
                  mat-icon-button
                  color="primary"
                  (click)="$event.stopPropagation(); onEdit.emit(element)"
                >
                  <mat-icon>edit</mat-icon>
                </button>
                <button
                  mat-icon-button
                  color="warn"
                  (click)="$event.stopPropagation(); onDelete.emit(element)"
                >
                  <mat-icon>delete</mat-icon>
                </button>
              </div>
              <span *ngSwitchDefault>{{ element[col.key] }}</span>
            </ng-container>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr
          mat-row
          *matRowDef="let row; columns: displayedColumns"
          (click)="onRowClick.emit(row)"
          class="hover:!bg-background cursor-pointer transition-theme"
        ></tr>
      </table>

      <mat-paginator
        [pageSizeOptions]="[10, 25, 100]"
        aria-label="Select page"
        class="!bg-surface !text-text-primary transition-theme"
      ></mat-paginator>
    </div>
  `,
})
export class DataTableComponent implements AfterViewInit {
  @Input() columns: TableColumn[] = [];
  @Input() set data(val: any[]) {
    this.dataSource.data = val;
  }
  @Output() onEdit = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();
  @Output() onRowClick = new EventEmitter<any>();

  displayedColumns: string[] = [];
  dataSource = new MatTableDataSource<any>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.displayedColumns = this.columns.map((c) => c.key);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
}
