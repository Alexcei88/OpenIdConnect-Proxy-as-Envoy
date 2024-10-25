import { Injectable } from '@angular/core';
import { AuthOptions } from '../auth-options';

@Injectable()
export class LogoffRevocationService {
  constructor(
  ) {}

  // Logs out on the server and the local client.
  // If the server state has changed, check session, then only a local logout.
  logoff(configId: string, authOptions?: AuthOptions): void {
      let currentLocation = window.location.href;
      window.location.href = "https://localhost:5001/ClientAppSettings/logout?backUrl=" + currentLocation;
  }
}
