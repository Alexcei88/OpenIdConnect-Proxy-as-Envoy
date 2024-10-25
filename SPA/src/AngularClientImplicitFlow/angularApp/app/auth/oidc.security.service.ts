import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthOptions } from './auth-options';
import { AuthStateService } from './authState/auth-state.service';
import { LoginResponse } from './login/login-response';
import { LogoffRevocationService } from './logoffRevoke/logoff-revocation.service';
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Injectable()
export class OidcSecurityService {
  /**
   * Emits each time an authorization event occurs.
   *
   * @returns In case of a single config it returns true if the user is authenticated and false if they are not.
   * If you are running multiple configs it returns an array with the configId and a boolean
   * if you are authenticated or not for this config
   */
  get isAuthenticated$(): Observable<boolean> {
    return this.authStateService.authenticated$;
  }

  constructor(
    private authStateService: AuthStateService,
    private logoffRevocationService: LogoffRevocationService
  ) {}
  
  /**
   * Returns the userData for a configuration
   *
   */
  getUserData(): any {
    return {
      "name": "Пеав"
    };
  }
  
  /**
   * Redirects the user to the STS to begin the authentication process.
   *
   * @param configId The configId to perform the action in behalf of. If not passed, the first configs will be taken
   * @param authOptions The custom options for the the authentication request.
   */
  authorize(): void {
    this.authStateService.setAuthenticatedAndFireEvent();
  }

  /**
   * Logs out on the server and the local client. If the server state has changed, confirmed via check session,
   * then only a local logout is performed.
   *
   */
  logoff(): void {
    
    this.logoffRevocationService.logoff();
    this.authStateService.setUnauthenticatedAndFireEvent();
  }
}
