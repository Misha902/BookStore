import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () => import('@abp/ng.tenant-management').then(c => c.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(c => c.createRoutes()),
  },
  {
    path: 'books',
    loadComponent: () =>
      import('./book/book.component').then(m => m.BookComponent),
    canActivate: [authGuard, permissionGuard]
  },
  {
    path: 'authors',
    loadComponent: () =>
      import('./author/author.component').then(m => m.AuthorComponent)
  },
  {
    path: 'orders',
    loadComponent: () => import('./order/order.component').then(m => m.OrderComponent),
  },
  {
    path: 'orders',
    loadComponent: () =>
      import('./order/order.component').then(m => m.OrderComponent),
  },
  {
    path: 'order/create',
    loadComponent: () =>
      import('./order/order-details/order-details.component').then(m => m.OrderDetailsComponent),
  },
  {
    path: 'order/details/:id',
    loadComponent: () =>
      import('./order/order-details/order-details.component').then(m => m.OrderDetailsComponent),
  },

];
