import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Order } from '../../shared/models/cart.model';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/orders`;

  createOrder(order: Partial<Order>): Observable<Order> {
    const newOrder = {
      ...order,
      id: Math.random().toString(36).substr(2, 9),
      createdAt: new Date(),
    } as Order;
    return of(newOrder);
  }

  getOrders(): Observable<Order[]> {
    return of([]);
  }
}
