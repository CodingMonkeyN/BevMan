import {inject, Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {from, Observable, switchMap} from 'rxjs';
import {SupabaseService} from "./supabase.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private readonly supabase = inject(SupabaseService);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return from(this.supabase.session).pipe(
      switchMap(session => {
        const authReq = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${session?.access_token ?? ''}`)
        });
        return next.handle(authReq);
      })
    )
  }
}
