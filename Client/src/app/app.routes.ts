import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'auth/login',
    loadComponent: () =>
      import('./modules/auth/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'pos',
        loadComponent: () => import('./modules/pos/pos.component').then((m) => m.PosComponent),
      },
      {
        path: 'products',
        loadComponent: () =>
          import('./modules/products/product-list/product-list.component').then(
            (m) => m.ProductListComponent,
          ),
      },
      {
        path: 'inventory',
        loadComponent: () =>
          import('./modules/inventory/inventory.component').then((m) => m.InventoryComponent),
      },
      {
        path: 'admin/users',
        loadComponent: () =>
          import('./modules/admin/user-list/user-list.component').then((m) => m.UserListComponent),
      },
      {
        path: '',
        redirectTo: 'pos',
        pathMatch: 'full',
      },
    ],
  },
];
