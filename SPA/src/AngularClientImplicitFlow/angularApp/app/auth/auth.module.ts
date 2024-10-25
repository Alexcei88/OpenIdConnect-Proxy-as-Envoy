import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, InjectionToken, ModuleWithProviders, NgModule, Provider } from '@angular/core';
import { DataService } from './api/data.service';
import { HttpBaseService } from './api/http-base.service';
import { AuthStateService } from './authState/auth-state.service';
import { LoggerService } from './logging/logger.service';
import { LogoffRevocationService } from './logoffRevoke/logoff-revocation.service';
import { OidcSecurityService } from './oidc.security.service';
import { PublicEventsService } from './public-events/public-events.service';

export interface PassedInitialConfig {
  loader?: Provider;
  storage?: any;
}

export const PASSED_CONFIG = new InjectionToken<PassedInitialConfig>('PASSED_CONFIG');

@NgModule({
  imports: [CommonModule, HttpClientModule],
  declarations: [],
  exports: [],
})
export class AuthModule {
  static forRoot(passedConfig: PassedInitialConfig): ModuleWithProviders<AuthModule> {
    return {
      ngModule: AuthModule,
      providers: [
        // Make the PASSED_CONFIG available through injection
        { provide: PASSED_CONFIG, useValue: passedConfig },
        PublicEventsService,
        OidcSecurityService,
        LogoffRevocationService,
        HttpBaseService,
        AuthStateService,
        LoggerService,
        DataService,
      ],
    };
  }
}
