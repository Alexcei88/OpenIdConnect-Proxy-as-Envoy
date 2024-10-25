import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Configuration } from '../app.constants';
import { DataEventRecord } from './models/DataEventRecord';

@Injectable()
export class DataEventRecordsService {

    private actionUrl: string;
    private headers: HttpHeaders = new HttpHeaders();

    constructor(private http: HttpClient, configuration: Configuration) {
        this.actionUrl = `${configuration.Server}api/DataEventRecords/`;
    }

    private setHeaders() {
        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
    }

    public GetAll = (): Observable<DataEventRecord[]> => {
        this.setHeaders();

        return this.http.get<DataEventRecord[]>(this.actionUrl, { headers: this.headers, withCredentials: true });
    }

    public GetById(id: number): Observable<DataEventRecord> {
        this.setHeaders();
        return this.http.get<DataEventRecord>(this.actionUrl + id, {
            headers: this.headers, withCredentials: true
        });
    }

    public Add(itemToAdd: any): Observable<any> {
        this.setHeaders();
        return this.http.post<any>(this.actionUrl, JSON.stringify(itemToAdd), { headers: this.headers, withCredentials: true });
    }

    public Update(id: number, itemToUpdate: any): Observable<any> {
        this.setHeaders();
        return this.http
            .put<any>(this.actionUrl + id, JSON.stringify(itemToUpdate), { headers: this.headers, withCredentials: true });
    }

    public Delete(id: number): Observable<any> {
        this.setHeaders();
        return this.http.delete<any>(this.actionUrl + id, {
            headers: this.headers, withCredentials: true
        });
    }
}
