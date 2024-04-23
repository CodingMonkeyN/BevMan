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
        path: 'tab1',
        loadComponent: () =>
          import('./../tab1/tab1.page').then((m) => m.Tab1Page),
      },
      {
        path: 'tab2',
        loadComponent: () =>
          import('./../tab2/tab2.page').then((m) => m.Tab2Page),
      },
      {
        path: 'tab3',
        loadComponent: () =>
          import('./../tab3/tab3.page').then((m) => m.Tab3Page),
      },
      {
        path: 'account',
        loadComponent: () =>
          import('./../account/account.page').then((m) => m.AccountPage),
      }
    ],
  }
];
