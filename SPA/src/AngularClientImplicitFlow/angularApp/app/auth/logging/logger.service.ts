import { Injectable } from '@angular/core';
import { ConfigurationProvider } from '../config/provider/config.provider';
import { LogLevel } from './log-level';

@Injectable()
export class LoggerService {
  constructor() {}

  logError(configId: string, message: any, ...args: any[]): void {
    if (this.loggingIsTurnedOff(configId)) {
      return;
    }

    if (!!args && args.length) {
      console.error(`[ERROR] ${configId} - ${message}`, ...args);
    } else {
      console.error(`[ERROR] ${configId} - ${message}`);
    }
  }

  logWarning(configId: string, message: any, ...args: any[]): void {
    if (!this.logLevelIsSet(configId)) {
      return;
    }

    if (this.loggingIsTurnedOff(configId)) {
      return;
    }

    if (!this.currentLogLevelIsEqualOrSmallerThan(configId, LogLevel.Warn)) {
      return;
    }

    if (!!args && args.length) {
      console.warn(`[WARN] ${configId} - ${message}`, ...args);
    } else {
      console.warn(`[WARN] ${configId} - ${message}`);
    }
  }

  logDebug(configId: string, message: any, ...args: any[]): void {
    if (!this.logLevelIsSet(configId)) {
      return;
    }

    if (this.loggingIsTurnedOff(configId)) {
      return;
    }

    if (!this.currentLogLevelIsEqualOrSmallerThan(configId, LogLevel.Debug)) {
      return;
    }

    if (!!args && args.length) {
      console.log(`[DEBUG] ${configId} - ${message}`, ...args);
    } else {
      console.log(`[DEBUG] ${configId} - ${message}`);
    }
  }

  private currentLogLevelIsEqualOrSmallerThan(configId: string, logLevelToCompare: LogLevel): boolean {
    
    return 0 <= logLevelToCompare;
  }

  private logLevelIsSet(configId: string): boolean {
    return false;
  }

  private loggingIsTurnedOff(configId: string): boolean {
    
    return 0 === LogLevel.None;
  }
}
