import { enableProdMode, importProvidersFrom, inject } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, RouteReuseStrategy } from '@angular/router';
import { IonicRouteStrategy, provideIonicAngular } from '@ionic/angular/standalone';

import { routes } from './app/app.routes';
import { environment } from './environments/environment';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './app/services/auth-interceptor.service';
import { ApiModule, Configuration } from './api';
import { TranslateCompiler, TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateMessageFormatCompiler } from 'ngx-translate-messageformat-compiler';
import { AppComponent } from './app/pages/app.component';
import { provideQueryClientOptions, QueryCache, QueryClientConfigFn } from '@ngneat/query';
import { NotificationService } from './app/services/notification.service';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

const withFunctionalFactory: QueryClientConfigFn = () => {
  const notificationService = inject(NotificationService);

  return {
    queryCache: new QueryCache({
      onError: (_: Error) => notificationService.showQueryError(),
    }),
    defaultOptions: {
      queries: {
        refetchInterval: 1000 * 60 * 5,
      },
    },
  };
};

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    provideQueryClientOptions(withFunctionalFactory),
    provideIonicAngular(),
    provideRouter(routes),
    importProvidersFrom(
      ApiModule.forRoot(() => {
        const config = new Configuration();
        config.basePath = environment.baseUrl;
        return config;
      }),
    ),
    importProvidersFrom(
      TranslateModule.forRoot({
        compiler: {
          provide: TranslateCompiler,
          useClass: TranslateMessageFormatCompiler,
        },
        loader: {
          provide: TranslateLoader,
          useFactory: createTranslateLoader,
          deps: [HttpClient],
        },
      }),
    ),
    importProvidersFrom(HttpClientModule),
  ],
});
