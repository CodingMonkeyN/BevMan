import {Routes} from '@angular/router';
import {TabsPage} from './tabs.page';
import {authGuard} from "../services/auth-guard.service";

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
            loadComponent: () => import('../product/product-page').then((m) => m.ProductPage),
          },
          {
            path: 'create',
            loadComponent: () => import('../product-editor/product-editor.component').then(m => m.ProductEditorComponent),
          },
          {
            path: ':id',
            loadComponent: () => import('../product-editor/product-editor.component').then(m => m.ProductEditorComponent)
          },
        ],
      },
      {
        path: 'users',
        children: [
          {
            loadComponent: () =>
              import('../user/user-page').then((m) => m.UserPage),
            path: '',
          },
          {
            loadComponent: () =>
              import('../user/user-details/user-details.component').then((m) => m.UserDetailsComponent),
            path: ':id',
          }
        ],
      },
      {
        path: 'account',
        children: [
          {
            path: '',
            loadComponent: () =>
              import('../account/account.page').then((m) => m.AccountPage),
          },
          {
            path: 'add-balance',
            loadComponent: () => import('../account/add-balance/add-balance.component').then((m) => m.AddBalanceComponent)
          }
        ],
      }
    ],
  }
];
