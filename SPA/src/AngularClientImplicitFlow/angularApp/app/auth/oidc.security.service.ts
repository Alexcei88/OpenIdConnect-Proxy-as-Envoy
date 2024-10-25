import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthOptions } from './auth-options';
import { AuthStateService } from './authState/auth-state.service';
import { LoginResponse } from './login/login-response';
import { LogoffRevocationService } from './logoffRevoke/logoff-revocation.service';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map} from "rxjs/operators";
import {Router} from "@angular/router";

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
    private logoffRevocationService: LogoffRevocationService,
    private httpClient: HttpClient,
    private router: Router
  ) {}

  checkAuth(): Observable<LoginResponse> {

    const httpGetOptions =
        {
          withCredentials: true,
          headers: new HttpHeaders (
                  {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                  }),
        };
    return this.httpClient.get<LoginResponse>("https://localhost:5001/ClientAppSettings/checkLogin", httpGetOptions)
        .pipe(map(response => {
            
            if (response == null || response.isAuthenticated)
                this.authStateService.setUnauthenticatedAndFireEvent();
            
            this.authStateService.setAuthenticatedAndFireEvent();
            this.router.navigate(['/dataeventrecords'])

            return response;
        }));
  }
  
  /**
   * Redirects the user to the STS to begin the authentication process.
   *
   * @param configId The configId to perform the action in behalf of. If not passed, the first configs will be taken
   * @param authOptions The custom options for the the authentication request.
   */
  authorize(): void {

    let currentLocation = window.location.href;
    window.location.href = "https://localhost:5001/ClientAppSettings/login?backUrl=" + currentLocation;
  }

  /**
   * Logs out on the server and the local client. If the server state has changed, confirmed via check session,
   * then only a local logout is performed.
   *
   * @param configId The configId to perform the action in behalf of. If not passed, the first configs will be taken
   * @param authOptions with custom parameters and/or an custom url handler
   */
  logoff(configId?: string, authOptions?: AuthOptions): void {
    
    this.logoffRevocationService.logoff(configId, authOptions);
    this.authStateService.setUnauthenticatedAndFireEvent();
  }
}
