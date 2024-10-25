import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpBaseService } from './http-base.service';

const NGSW_CUSTOM_PARAM = 'ngsw-bypass';

@Injectable()
export class DataService {
  constructor(private httpClient: HttpBaseService) {}

  get<T>(url: string, configId: string, token?: string): Observable<T> {
    const headers = this.prepareHeaders(token);
    
    return this.httpClient.get<T>(url, {
      headers
    });
  }

  post<T>(url: string, body: any, configId: string, headersParams?: HttpHeaders): Observable<T> {
    const headers = headersParams || this.prepareHeaders();
    
    return this.httpClient.post<T>(url, body, { headers });
  }

  private prepareHeaders(token?: string): HttpHeaders {
    let headers = new HttpHeaders();
    headers = headers.set('Accept', 'application/json');

    if (!!token) {
      headers = headers.set('Authorization', 'Bearer ' + decodeURIComponent(token));
    }

    return headers;
  }
}
