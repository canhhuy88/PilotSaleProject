import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../core/services/cart.service';
import { CartItem } from '../../shared/models/cart.model';

@Component({
  selector: 'app-pos-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pos-cart.component.html',
  styleUrls: ['./pos-cart.component.scss'],
})
export class PosCartComponent {
  private cartService = inject(CartService);

  items = this.cartService.items;
  totalPrice = this.cartService.totalPrice;

  incrementQuantity(item: CartItem): void {
    if (item.quantity < item.product.stock) {
      this.cartService.updateQuantity(item.product.id, item.quantity + 1);
    }
  }

  decrementQuantity(item: CartItem): void {
    if (item.quantity > 1) {
      this.cartService.updateQuantity(item.product.id, item.quantity - 1);
    } else {
      this.cartService.removeFromCart(item.product.id);
    }
  }

  removeItem(productId: string): void {
    this.cartService.removeFromCart(productId);
  }

  clearCart(): void {
    this.cartService.clearCart();
  }
}
