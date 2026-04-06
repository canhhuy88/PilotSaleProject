export interface Product {
  id: string;
  name: string;
  price: number;
  stock: number;
  imageUrl?: string;
  category?: string;
  sku?: string;
}
