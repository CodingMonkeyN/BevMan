import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from 'rxjs';
import {SupabaseService} from "../app/supabase.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private accessToken?: string;

  constructor(supabase: SupabaseService) {
    supabase.authChanges((authChangeEvent, session) => {
      if (authChangeEvent === "TOKEN_REFRESHED") {
        this.accessToken = session?.access_token;
      }
    })
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', this.accessToken ?? '')
    });
    return next.handle(authReq);
  }
}
