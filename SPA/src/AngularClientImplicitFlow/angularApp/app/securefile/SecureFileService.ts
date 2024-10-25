import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Configuration } from '../app.constants';
import { OidcSecurityService } from '../auth/angular-auth-oidc-client';

@Injectable()
export class SecureFileService {

    private actionUrl: string;
    private fileExplorerUrl: string;
    private headers: HttpHeaders = new HttpHeaders();

    constructor(private http: HttpClient, _configuration: Configuration, public oidcSecurityService: OidcSecurityService) {
        this.actionUrl = `${_configuration.FileServer}resourceFileServer/api/Download/`;
        this.fileExplorerUrl = `${_configuration.FileServer }resourceFileServer/api/FileExplorer/`;
    }

    public DownloadFile(id: string) {
        this.setHeaders();
        let oneTimeAccessToken = '';

        this.http.get(`${this.actionUrl}GenerateOneTimeAccessToken/${id}`, {
            headers: this.headers
        }).subscribe(
            (data: any) => {
                oneTimeAccessToken = data.oneTimeToken;
                console.log(`open DownloadFile for file ${id}: ${this.actionUrl}${oneTimeAccessToken}/${id}`);
                window.open(`${this.actionUrl}${oneTimeAccessToken}/${id}`);
            });
    }

    public GetListOfFiles = (): Observable<string[]> => {
        this.setHeaders();

        return this.http.get<string[]>(this.fileExplorerUrl, { headers: this.headers });
    }

    private setHeaders() {
        this.headers = new HttpHeaders();
        this.headers = this.headers.set('Content-Type', 'application/json');
        this.headers = this.headers.set('Accept', 'application/json');
    }
}
