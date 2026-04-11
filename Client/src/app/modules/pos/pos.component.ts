import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ProductService } from '../../core/services/product.service';
import { CartService } from '../../core/services/cart.service';
import { OrderService } from '../../core/services/order.service';
import { NotificationService } from '../../shared/services/notification.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { Product } from '../../shared/models/product.model';
import { CartItem } from '../../shared/models/cart.model';
import { categoryApi, authApi } from '../../../api/authApi';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-pos',
  standalone: true,
  imports: [
    CommonModule,
    MatGridListModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatBadgeModule,
    MatDialogModule,
  ],
  template: `
    <div class="h-full flex gap-6 overflow-hidden">
      <!-- Left: Products List -->
      <div class="flex-1 flex flex-col overflow-hidden">
        <div class="flex items-center justify-between mb-4">
          <h1 class="text-2xl font-bold text-gray-800">Products</h1>
          <div class="flex gap-2">
            <button mat-stroked-button class="rounded-lg">All Categories</button>
            <div class="relative">
              <input
                type="text"
                placeholder="Search products..."
                class="pl-10 pr-4 py-2 rounded-lg border border-gray-200 focus:outline-none focus:ring-2 focus:ring-primary-500 w-64"
              />
              <mat-icon class="absolute left-3 top-2.5 text-gray-400 text-sm">search</mat-icon>
            </div>
          </div>
        </div>

        <div class="flex-1 overflow-auto pr-2 scrollbar-hide">
          <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 pb-4">
            <mat-card
              *ngFor="let product of products$ | async"
              (click)="addToCart(product)"
              class="cursor-pointer hover:shadow-md transition-shadow rounded-xl overflow-hidden border-0 bg-white"
            >
              <div class="aspect-square bg-gray-100 flex items-center justify-center relative">
                <mat-icon class="scale-[2.5] text-gray-300">image</mat-icon>
                <div
                  *ngIf="product.stock <= 5"
                  class="absolute top-2 right-2 px-2 py-1 bg-red-100 text-red-600 text-[10px] font-bold rounded-full"
                >
                  LOW STOCK: {{ product.stock }}
                </div>
              </div>
              <mat-card-content class="p-3">
                <h3 class="font-semibold text-gray-800 truncate">{{ product.name }}</h3>
                <div class="flex justify-between items-center mt-1">
                  <span class="text-primary-600 font-bold">{{ product.price | currency }}</span>
                  <span class="text-xs text-gray-400">{{ product.sku }}</span>
                </div>
              </mat-card-content>
            </mat-card>
          </div>
        </div>
      </div>

      <!-- Right: Cart -->
      <div
        class="w-96 flex flex-col bg-white border border-gray-100 rounded-2xl shadow-sm overflow-hidden"
      >
        <div class="p-4 border-b border-gray-50 flex items-center justify-between bg-gray-50/50">
          <div class="flex items-center gap-2">
            <h2 class="text-lg font-bold text-gray-800">Current Order</h2>
            <span
              class="bg-primary-100 text-primary-700 text-xs px-2 py-0.5 rounded-full font-bold"
            >
              {{ cartService.totalItems() }} items
            </span>
          </div>
          <button mat-icon-button (click)="clearCart()" class="text-gray-400">
            <mat-icon>delete_sweep</mat-icon>
          </button>
        </div>

        <div class="flex-1 overflow-auto p-4 space-y-4">
          <div
            *ngIf="cartItems().length === 0"
            class="h-full flex flex-col items-center justify-center text-gray-400"
          >
            <mat-icon class="scale-[3] mb-4 opacity-20">shopping_basket</mat-icon>
            <p>Your cart is empty</p>
          </div>

          <div *ngFor="let item of cartItems()" class="flex gap-3">
            <div
              class="w-12 h-12 rounded-lg bg-gray-100 flex items-center justify-center flex-shrink-0"
            >
              <mat-icon class="text-gray-300">inventory_2</mat-icon>
            </div>
            <div class="flex-1 min-w-0">
              <h4 class="font-medium text-gray-800 truncate text-sm">{{ item.product.name }}</h4>
              <div class="flex items-center justify-between mt-1">
                <div class="flex items-center gap-2">
                  <button
                    (click)="decreaseQty(item)"
                    class="w-6 h-6 rounded-md bg-gray-100 flex items-center justify-center hover:bg-gray-200"
                  >
                    -
                  </button>
                  <span class="text-sm font-bold w-6 text-center">{{ item.quantity }}</span>
                  <button
                    (click)="increaseQty(item)"
                    class="w-6 h-6 rounded-md bg-gray-100 flex items-center justify-center hover:bg-gray-200"
                  >
                    +
                  </button>
                </div>
                <p class="font-bold text-gray-800 text-sm">
                  {{ item.product.price * item.quantity | currency }}
                </p>
              </div>
            </div>
            <button
              (click)="removeFromCart(item.product.id)"
              class="text-gray-300 hover:text-red-500 flex-shrink-0 self-center"
            >
              <mat-icon class="text-sm">close</mat-icon>
            </button>
          </div>
        </div>

        <div class="p-6 bg-gray-50/50 border-t border-gray-100 space-y-3">
          <div class="flex justify-between text-gray-500 text-sm">
            <span>Subtotal</span>
            <span>{{ cartService.totalPrice() | currency }}</span>
          </div>
          <div class="flex justify-between text-gray-500 text-sm">
            <span>Tax (0%)</span>
            <span>$0.00</span>
          </div>
          <mat-divider class="my-2"></mat-divider>
          <div class="flex justify-between text-gray-900 font-bold text-xl">
            <span>Total</span>
            <span class="text-primary-600">{{ cartService.totalPrice() | currency }}</span>
          </div>

          <button
            mat-flat-button
            color="primary"
            class="w-full h-14 rounded-xl !text-lg !font-bold shadow-lg shadow-primary-100 mt-4"
            [disabled]="cartItems().length === 0"
            (click)="checkout()"
          >
            Checkout
          </button>
        </div>
      </div>
    </div>
  `,
})
export class PosComponent implements OnInit, OnDestroy {
  productService = inject(ProductService);
  cartService = inject(CartService);
  orderService = inject(OrderService);
  notification = inject(NotificationService);
  dialog = inject(MatDialog);
  private authService = inject(AuthService);

  products$ = this.productService.getProducts();
  cartItems = this.cartService.items;
  private intervalId: any;

  constructor() {
    console.log('Cart items:111111111111');
  }

  ngOnInit() {
    this.intervalId = setInterval(() => {
      categoryApi
        .getCategories()
        .then((categories) => {
          console.log('Categories:', categories);
        })
        .catch((err) => {
          console.error('Failed to fetch categories:', err);
        });
    }, 10000);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  addToCart(product: Product) {
    this.cartService.addToCart(product);
  }

  increaseQty(item: CartItem) {
    this.cartService.updateQuantity(item.product.id, item.quantity + 1);
  }

  decreaseQty(item: CartItem) {
    this.cartService.updateQuantity(item.product.id, item.quantity - 1);
  }

  removeFromCart(productId: string) {
    this.cartService.removeFromCart(productId);
  }

  clearCart() {
    if (this.cartItems().length > 0) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'Clear Cart',
          message: 'Are you sure you want to remove all items from the cart?',
          primaryText: 'Clear All',
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.cartService.clearCart();
        }
      });
    }
  }

  checkout() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Complete Order',
        message: `Total to pay: ${new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(this.cartService.totalPrice())}`,
        primaryText: 'Finish Order',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.orderService
          .createOrder({ items: this.cartItems(), total: this.cartService.totalPrice() })
          .subscribe(() => {
            this.notification.success('Order completed successfully!');
            this.cartService.clearCart();
          });
      }
    });
  }
}
