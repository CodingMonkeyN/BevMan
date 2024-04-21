import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  PreloadAllModules,
  RouterModule,
  RouterStateSnapshot,
  Routes
} from '@angular/router'
import {inject, NgModule} from '@angular/core'
import {SupabaseService} from "./supabase.service";

const AuthGuard: CanActivateFn = async()  =>  {
  return !!(await inject(SupabaseService).session)
}

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./login/login.page').then( m => m.LoginPage)
  },
  {
    path: 'account',
    loadComponent: () => import('./account/account.page').then( m => m.AccountPage),
    canActivate: [AuthGuard]
  },

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
