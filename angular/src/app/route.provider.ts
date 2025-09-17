import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
    },
    {
        name: '::Menu:BookStore',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'BookStore.Books || BookStore.Authors'
    },
    {
        path: '/books',
        name: '::Menu:Books',
        parentName: '::Menu:BookStore',
        iconClass: 'fas fa-book-open',
        layout: eLayoutType.application,
        requiredPolicy: 'BookStore.Books',
    },
    {
        path: '/authors',
        name: '::Menu:Authors',
        parentName: '::Menu:BookStore',
        iconClass: 'fas fa-user-edit',
        layout: eLayoutType.application,
        requiredPolicy: 'BookStore.Authors',
    },
    {
        path: '/orders',
        name: 'Orders',
        iconClass: 'fas fa-shopping-cart',
        order: 3,
        layout: eLayoutType.application,
    },
  ]);
}
