import {inject, Injectable} from '@angular/core';
import {Router, UrlTree} from "@angular/router";
import {SupabaseService} from "./supabase.service";

export const authGuard = () : Promise<boolean | UrlTree> => inject(AuthGuardService).canActivate()
@Injectable({
  providedIn: 'root'
})
export class AuthGuardService{
  constructor(private authService: SupabaseService, private router: Router) {}

  async canActivate(): Promise<boolean | UrlTree> {
    if (!await this.authService.session) {
      return this.router.parseUrl('/login');
    } else {
      return true;
    }
  }
}
