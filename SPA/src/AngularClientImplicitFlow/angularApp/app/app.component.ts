import {
    OidcSecurityService,
} from './auth/angular-auth-oidc-client';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LocaleService, TranslationService, Language } from 'angular-l10n';
import './app.component.css';
import {AuthStateService} from "./auth/authState/auth-state.service";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Component({
    selector: 'app-component',
    templateUrl: 'app.component.html',
})

export class AppComponent implements OnInit {

    @Language() lang = '';

    title = '';
    userName = '';
    isAuthenticated$: Observable<boolean>;

    constructor(
        public oidcSecurityService: OidcSecurityService,
        public locale: LocaleService,
        public translation: TranslationService,
        private authStateService: AuthStateService,
        private router: Router,
        private httpClient: HttpClient
) {
        console.log('AppComponent STARTING');
    }

    ngOnInit() {
        this.authStateService.setAuthenticatedAndFireEvent();
       
        this.isAuthenticated$ = this.oidcSecurityService.isAuthenticated$;
        this.isAuthenticated$.subscribe((isAuthentificated) => {
            if (isAuthentificated) {
                this.httpClient.get<any>("https://localhost:14100/api/ClientAppSettings/username")
                    .subscribe((response) => this.userName = response.name)
            }
        });

        this.router.navigate(['/dataeventrecords']);
    }
    
    login() {
        console.log('start login');

        let culture = 'de-CH';
        if (this.locale.getCurrentCountry()) {
            culture = this.locale.getCurrentLanguage() + '-' + this.locale.getCurrentCountry();
        }
        console.log(culture);

        this.oidcSecurityService.authorize();
    }

    refreshSession() {
        console.log('start refreshSession');
        this.oidcSecurityService.authorize();
    }

    logout() {
        console.log('start logoff');
        this.oidcSecurityService.logoff();
    }

    getUserName() {
        return this.userName;
    }
}
