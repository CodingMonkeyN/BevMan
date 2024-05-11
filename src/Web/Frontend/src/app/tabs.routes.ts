import { Routes } from '@angular/router';
import { TabsPage } from './pages/tabs/tabs.page';
import { authGuard } from './services/auth-guard.service';

export const tabsRoutes: Routes = [
  {
    path: 'tabs',
    component: TabsPage,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'products',
        pathMatch: 'full',
      },
      {
        path: 'products',
        children: [
          {
            path: '',
            loadComponent: () => import('./pages/product/product-page').then(m => m.ProductPage),
          },
          {
            path: ':id',
            loadComponent: () =>
              import('./pages/product/product-details/product-details.page').then(m => m.ProductDetailsPage),
          },
        ],
      },
      {
        path: 'balance-requests',
        loadComponent: () => import('./pages/balance-request/balance-request.page').then(m => m.BalanceRequestPage),
      },
      {
        path: 'users',
        children: [
          {
            loadComponent: () => import('./pages/user/user-page').then(m => m.UserPage),
            path: '',
          },
          {
            loadComponent: () =>
              import('./pages/user/user-details/user-details.component').then(m => m.UserDetailsComponent),
            path: ':id',
          },
        ],
      },
      {
        path: 'account',
        children: [
          {
            path: '',
            loadComponent: () => import('./pages/account/account.page').then(m => m.AccountPage),
          },
          {
            path: 'add-balance',
            loadComponent: () => import('./pages/account/add-balance/add-balance.page').then(m => m.AddBalancePage),
          },
        ],
      },
    ],
  },
];
