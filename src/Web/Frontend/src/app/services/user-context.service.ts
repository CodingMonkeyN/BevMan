import { Injectable, Signal } from '@angular/core';
import { SupabaseService } from './supabase.service';
import { filter, map, Subject, switchMap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({ providedIn: 'root' })
export class UserContext {
  readonly roles: Signal<string[] | undefined>;
  private readonly refresh = new Subject<void>();

  constructor(private readonly supabaseService: SupabaseService) {
    this.supabaseService.authChanges(() => {
      this.refresh.next();
    });

    this.roles = toSignal(
      this.refresh.pipe(
        switchMap(() => this.supabaseService.session),
        filter(session => !!session),
        map(session => (session?.access_token ? jwtDecode(session?.access_token) : undefined)),
        map(decoded => (decoded as { user_roles: string[] }).user_roles),
      ),
    );
  }

  hasRoles(roles: string[]): boolean {
    return this.roles()?.some(role => roles.includes(role)) ?? false;
  }
}
