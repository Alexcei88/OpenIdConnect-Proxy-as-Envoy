import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, retry, switchMap, tap } from 'rxjs/operators';
import { DataService } from '../api/data.service';
import { AuthOptions } from '../auth-options';
import { LoggerService } from '../logging/logger.service';

@Injectable()
export class LogoffRevocationService {
  constructor(
    private httpClient: HttpClient
  ) {}

  // Logs out on the server and the local client.
  // If the server state has changed, check session, then only a local logout.
  logoff(): void {

    this.httpClient
        .get<any>(`https://localhost:14100/api/ClientAppSettings/logoutUrl`)
        .subscribe((response) => {
            
            this.httpClient
               .post<any>("https://localhost:14100/oauth2/signout", "", { observe: 'response'})
                .pipe(catchError((error: any, caught: Observable<any>): Observable<any> => {
                  window.location.href = response.url;
                  return of();
                }))
               .subscribe(
                   () => window.location.href = response.url)});
  }
}
