import {enableProdMode, importProvidersFrom} from '@angular/core';
import {bootstrapApplication} from '@angular/platform-browser';
import {provideRouter, RouteReuseStrategy} from '@angular/router';
import {IonicRouteStrategy, provideIonicAngular} from '@ionic/angular/standalone';

import {routes} from './app/app.routes';
import {AppComponent} from './app/app.component';
import {environment} from './environments/environment';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from "@angular/common/http";
import {AuthInterceptor} from "./app/services/auth-interceptor.service";
import {ApiModule, Configuration} from "./api";
import {TranslateCompiler, TranslateLoader, TranslateModule} from "@ngx-translate/core";
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {TranslateMessageFormatCompiler} from "ngx-translate-messageformat-compiler";

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    {provide: RouteReuseStrategy, useClass: IonicRouteStrategy},
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    provideIonicAngular(),
    provideRouter(routes),
    importProvidersFrom(ApiModule.forRoot(() => {
      const config = new Configuration();
      config.basePath = environment.baseUrl;
      return config;
    })),
    importProvidersFrom(TranslateModule.forRoot({
      compiler: {
        provide: TranslateCompiler,
        useClass: TranslateMessageFormatCompiler,
      },
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient],
      },
    })),
    importProvidersFrom(HttpClientModule),
  ]
});
