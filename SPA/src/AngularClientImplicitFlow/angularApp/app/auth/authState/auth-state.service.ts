import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { distinctUntilChanged } from 'rxjs/operators';
import { LoggerService } from '../logging/logger.service';
import { EventTypes } from '../public-events/event-types';
import { PublicEventsService } from '../public-events/public-events.service';

@Injectable()
export class AuthStateService {
  private authenticatedInternal$ = new BehaviorSubject<boolean>(null);

  get authenticated$(): Observable<boolean> {
    return this.authenticatedInternal$.asObservable().pipe(distinctUntilChanged());
  }

  constructor(
    private loggerService: LoggerService,
    private publicEventsService: PublicEventsService,
  ) {}

  setAuthenticatedAndFireEvent(): void {
    this.authenticatedInternal$.next(true);
  }

  setUnauthenticatedAndFireEvent(): void {
    
    this.authenticatedInternal$.next(false);
  }

  updateAndPublishAuthState(authorizationResult: boolean): void {
    this.publicEventsService.fireEvent<boolean>(EventTypes.NewAuthorizationResult, authorizationResult);
  }

  setAuthorizationData(accessToken: string, configId: string): void {
    this.loggerService.logDebug(configId, `storing the accessToken '${accessToken}'`);

    this.setAuthenticatedAndFireEvent();
  }

}
