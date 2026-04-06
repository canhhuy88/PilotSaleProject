import { Injectable, signal, computed } from '@angular/core';
import { Product } from '../../shared/models/product.model';
import { CartItem } from '../../shared/models/cart.model';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private _items = signal<CartItem[]>([]);
  items = this._items.asReadonly();

  totalItems = computed(() => this._items().reduce((acc, item) => acc + item.quantity, 0));
  totalPrice = computed(() =>
    this._items().reduce((acc, item) => acc + item.product.price * item.quantity, 0),
  );

  addToCart(product: Product) {
    const currentItems = this._items();
    const existingItem = currentItems.find((item) => item.product.id === product.id);

    if (existingItem) {
      if (existingItem.quantity < product.stock) {
        this.updateQuantity(product.id, existingItem.quantity + 1);
      }
    } else {
      this._items.set([...currentItems, { product, quantity: 1 }]);
    }
  }

  removeFromCart(productId: string) {
    this._items.set(this._items().filter((item) => item.product.id !== productId));
  }

  updateQuantity(productId: string, quantity: number) {
    if (quantity <= 0) {
      this.removeFromCart(productId);
      return;
    }

    this._items.set(
      this._items().map((item) => {
        if (item.product.id === productId) {
          return { ...item, quantity };
        }
        return item;
      }),
    );
  }

  clearCart() {
    this._items.set([]);
  }
}
