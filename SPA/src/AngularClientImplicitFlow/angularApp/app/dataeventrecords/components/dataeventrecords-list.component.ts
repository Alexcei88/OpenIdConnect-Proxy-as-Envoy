import { Component, OnInit } from '@angular/core';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { OidcSecurityService } from '../../auth/angular-auth-oidc-client';

import { DataEventRecordsService } from '../dataeventrecords.service';
import { DataEventRecord } from '../models/DataEventRecord';

@Component({
    selector: 'app-dataeventrecords-list',
    templateUrl: 'dataeventrecords-list.component.html'
})

export class DataEventRecordsListComponent implements OnInit {

    message: string;
    DataEventRecords: DataEventRecord[] = [];
    isAuthenticated$: Observable<boolean>;

    constructor(

        private dataEventRecordsService: DataEventRecordsService,
        public oidcSecurityService: OidcSecurityService,
    ) {
        this.message = 'DataEventRecords';
    }

    ngOnInit() {
        this.isAuthenticated$ = this.oidcSecurityService.isAuthenticated$;

        this.isAuthenticated$.pipe(
            switchMap((isAuthorized) => this.getData(isAuthorized as boolean))
        ).subscribe(
            data => this.DataEventRecords = data,
            () => console.log('getData Get all completed')
        );
    }

    Delete(id: any) {
        console.log('Try to delete' + id);
        this.dataEventRecordsService.Delete(id).pipe(
            switchMap(() => this.getData(true))
        ).subscribe((data) => this.DataEventRecords = data,
            () => console.log('getData Get all completed')
        );
    }

    private getData(isAuthenticated: boolean): Observable<DataEventRecord[]> {
        if (isAuthenticated) {
            return this.dataEventRecordsService.GetAll();
        }
        return of(null);
    }
}
