import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import {
  DataTableComponent,
  TableColumn,
} from '../../shared/components/data-table/data-table.component';
import { InventoryService } from '../../core/services/inventory.service';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, DataTableComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-2xl font-bold text-gray-800">Inventory Control</h1>
          <p class="text-gray-500">Monitor stock levels across all locations</p>
        </div>
        <div class="flex gap-3">
          <button mat-stroked-button class="rounded-lg h-11">
            <mat-icon class="mr-2">file_download</mat-icon>
            Export
          </button>
          <button
            mat-flat-button
            color="primary"
            class="rounded-lg h-11 px-6 shadow-md shadow-primary-100"
          >
            <mat-icon class="mr-2">add</mat-icon>
            Import Stock
          </button>
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-6">
        <div class="bg-white p-6 rounded-2xl border border-gray-100 shadow-sm">
          <p class="text-gray-500 text-sm font-medium">Total Items</p>
          <h3 class="text-2xl font-bold text-gray-800 mt-1">1,248</h3>
          <div class="mt-2 text-green-600 text-xs font-bold">+12% from last month</div>
        </div>
        <div class="bg-white p-6 rounded-2xl border border-gray-100 shadow-sm">
          <p class="text-gray-500 text-sm font-medium">Low Stock Alerts</p>
          <h3 class="text-2xl font-bold text-red-600 mt-1">14</h3>
          <div class="mt-2 text-gray-400 text-xs">Immediate action required</div>
        </div>
        <div class="bg-white p-6 rounded-2xl border border-gray-100 shadow-sm">
          <p class="text-gray-500 text-sm font-medium">Out of Stock</p>
          <h3 class="text-2xl font-bold text-gray-400 mt-1">3</h3>
          <div class="mt-2 text-blue-600 text-xs font-bold">5 items arriving tomorrow</div>
        </div>
      </div>

      <app-data-table [columns]="columns" [data]="(inventory$ | async) || []"></app-data-table>
    </div>
  `,
})
export class InventoryComponent {
  private inventoryService = inject(InventoryService);

  inventory$ = this.inventoryService.getInventory();

  columns: TableColumn[] = [
    { key: 'sku', label: 'SKU' },
    { key: 'productName', label: 'Product' },
    { key: 'stock', label: 'Current Stock', type: 'number' },
    { key: 'location', label: 'Location' },
    { key: 'actions', label: 'Actions', type: 'actions' },
  ];
}
