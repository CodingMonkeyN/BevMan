import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  PreloadAllModules, Router,
  RouterModule,
  RouterStateSnapshot,
  Routes
} from '@angular/router'
import { NgModule} from '@angular/core'
import {SupabaseService} from "./supabase.service";
import {tabsRoutes} from "./tabs/tabs.routes";
import {authGuard} from "./services/auth-guard.service";


export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./login/login.page').then( m => m.LoginPage)
  },
  ...tabsRoutes
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      preloadingStrategy: PreloadAllModules,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
