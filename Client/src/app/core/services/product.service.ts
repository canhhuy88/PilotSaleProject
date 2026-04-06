import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/products`;

  // Mock data for demo
  private mockProducts: Product[] = [
    {
      id: '1',
      name: 'Premium Coffee Beans',
      price: 15.99,
      stock: 50,
      category: 'Beverages',
      sku: 'COF-001',
    },
    {
      id: '2',
      name: 'Organic Green Tea',
      price: 12.5,
      stock: 100,
      category: 'Beverages',
      sku: 'TEA-001',
    },
    {
      id: '3',
      name: 'Eco-friendly Mug',
      price: 8.0,
      stock: 25,
      category: 'Merchandise',
      sku: 'MUG-001',
    },
    {
      id: '4',
      name: 'French Press',
      price: 35.0,
      stock: 10,
      category: 'Equipment',
      sku: 'EQU-001',
    },
    {
      id: '5',
      name: 'Artisan Chocolate Bar',
      price: 4.99,
      stock: 150,
      category: 'Snacks',
      sku: 'CHO-001',
    },
  ];

  getProducts(): Observable<Product[]> {
    return of(this.mockProducts);
  }

  getProductById(id: string): Observable<Product> {
    const product = this.mockProducts.find((p) => p.id === id);
    return of(product!);
  }

  createProduct(product: Partial<Product>): Observable<Product> {
    const newProduct = { ...product, id: Math.random().toString(36).substr(2, 9) } as Product;
    this.mockProducts.push(newProduct);
    return of(newProduct);
  }

  updateProduct(id: string, product: Partial<Product>): Observable<Product> {
    const index = this.mockProducts.findIndex((p) => p.id === id);
    if (index !== -1) {
      this.mockProducts[index] = { ...this.mockProducts[index], ...product };
      return of(this.mockProducts[index]);
    }
    throw new Error('Product not found');
  }

  deleteProduct(id: string): Observable<void> {
    const index = this.mockProducts.findIndex((p) => p.id === id);
    if (index !== -1) {
      this.mockProducts.splice(index, 1);
    }
    return of(undefined);
  }
}
