import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import {
  DataTableComponent,
  TableColumn,
} from '../../../shared/components/data-table/data-table.component';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ProductDialogComponent } from '../product-dialog/product-dialog.component';
import { Product } from '../../../shared/models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, MatDialogModule, DataTableComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <div>
          <h1 class="text-2xl font-bold text-gray-800">Product Management</h1>
          <p class="text-gray-500">Manage your product catalog and pricing</p>
        </div>
        <button
          mat-flat-button
          color="primary"
          (click)="addProduct()"
          class="rounded-lg h-11 px-6 shadow-md shadow-primary-100"
        >
          <mat-icon class="mr-2">add</mat-icon>
          Add Product
        </button>
      </div>

      <app-data-table
        [columns]="columns"
        [data]="(products$ | async) || []"
        (onEdit)="editProduct($event)"
        (onDelete)="deleteProduct($event)"
      ></app-data-table>
    </div>
  `,
})
export class ProductListComponent {
  private productService = inject(ProductService);
  private notification = inject(NotificationService);
  private dialog = inject(MatDialog);

  products$ = this.productService.getProducts();

  columns: TableColumn[] = [
    { key: 'sku', label: 'SKU' },
    { key: 'name', label: 'Product Name' },
    { key: 'category', label: 'Category' },
    { key: 'price', label: 'Price', type: 'currency' },
    { key: 'stock', label: 'Stock', type: 'number' },
    { key: 'actions', label: 'Actions', type: 'actions' },
  ];

  addProduct() {
    this.openDialog();
  }

  editProduct(product: Product) {
    this.openDialog(product);
  }

  deleteProduct(product: Product) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Product',
        message: `Are you sure you want to delete ${product.name}?`,
        primaryColor: 'warn',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.productService.deleteProduct(product.id).subscribe(() => {
          this.notification.success('Product deleted successfully');
          this.products$ = this.productService.getProducts();
        });
      }
    });
  }

  private openDialog(product?: Product) {
    const dialogRef = this.dialog.open(ProductDialogComponent, {
      width: '500px',
      data: product,
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.products$ = this.productService.getProducts();
      }
    });
  }
}
