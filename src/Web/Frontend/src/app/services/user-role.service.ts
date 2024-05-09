import {Injectable, Signal} from '@angular/core';
import {SupabaseService} from "./supabase.service";
import {from, map} from "rxjs";
import {jwtDecode} from "jwt-decode";
import {toSignal} from "@angular/core/rxjs-interop";

@Injectable({  providedIn: 'root'})
export class UserRoleService {
 readonly roles: Signal<string[] | undefined>
  constructor(private readonly supabaseService: SupabaseService) {
   this.roles = toSignal(from(this.supabaseService.session).pipe(
      map(session => session?.access_token ? jwtDecode(session?.access_token) : undefined),
     map(decoded => (decoded as {user_roles: string[]}).user_roles),
   ));
  }

  hasRoles(roles: string[]): boolean {
    return this.roles()?.some(role => roles.includes(role)) ?? false;
  }
}
