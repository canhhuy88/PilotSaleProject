import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/inventory`;

  getInventory(): Observable<any[]> {
    // Mock data
    return of([
      {
        id: '1',
        productName: 'Premium Coffee Beans',
        sku: 'COF-001',
        stock: 50,
        location: 'Warehouse A',
      },
      {
        id: '2',
        productName: 'Organic Green Tea',
        sku: 'TEA-001',
        stock: 100,
        location: 'Warehouse A',
      },
      {
        id: '3',
        productName: 'Eco-friendly Mug',
        sku: 'MUG-001',
        stock: 25,
        location: 'Warehouse B',
      },
    ]);
  }

  adjustStock(productId: string, adjustment: number): Observable<void> {
    return of(undefined);
  }
}
