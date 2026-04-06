import { Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../shared/services/notification.service';
import { Product } from '../../../shared/models/product.model';

@Component({
  selector: 'app-product-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit Product' : 'Add Product' }}</h2>
    <form [formGroup]="productForm" (ngSubmit)="onSubmit()">
      <mat-dialog-content class="flex flex-col gap-4">
        <mat-form-field appearance="outline">
          <mat-label>Product Name</mat-label>
          <input matInput formControlName="name" placeholder="Enter product name" />
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>SKU</mat-label>
          <input matInput formControlName="sku" placeholder="Enter SKU" />
        </mat-form-field>

        <div class="grid grid-cols-2 gap-4">
          <mat-form-field appearance="outline">
            <mat-label>Price</mat-label>
            <input matInput type="number" formControlName="price" placeholder="0.00" />
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Stock</mat-label>
            <input matInput type="number" formControlName="stock" placeholder="0" />
          </mat-form-field>
        </div>

        <mat-form-field appearance="outline">
          <mat-label>Category</mat-label>
          <input matInput formControlName="category" placeholder="Enter category" />
        </mat-form-field>
      </mat-dialog-content>

      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="dialogRef.close()">Cancel</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="productForm.invalid">
          {{ data ? 'Update' : 'Create' }}
        </button>
      </mat-dialog-actions>
    </form>
  `,
})
export class ProductDialogComponent {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private notification = inject(NotificationService);

  productForm = this.fb.group({
    name: ['', [Validators.required]],
    sku: ['', [Validators.required]],
    price: [0, [Validators.required, Validators.min(0)]],
    stock: [0, [Validators.required, Validators.min(0)]],
    category: [''],
  });

  constructor(
    public dialogRef: MatDialogRef<ProductDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Product,
  ) {
    if (data) {
      this.productForm.patchValue(data);
    }
  }

  onSubmit() {
    if (this.productForm.valid) {
      const product = this.productForm.value;
      const operation = this.data
        ? this.productService.updateProduct(this.data.id, product as Partial<Product>)
        : this.productService.createProduct(product as Partial<Product>);

      operation.subscribe(() => {
        this.notification.success(`Product ${this.data ? 'updated' : 'created'} successfully`);
        this.dialogRef.close(true);
      });
    }
  }
}
