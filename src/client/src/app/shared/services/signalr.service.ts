import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
  JsonHubProtocol
} from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

import { BaseEvent } from '../events/base-event';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  baseUrl: string = `${environment.backendUrl}/api`;

  private hubConnection: HubConnection | undefined;

  constructor() { }

  public async startConnection(): Promise<void> {
    this.stopConnection();

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.baseUrl, {
        transport: HttpTransportType.WebSockets
      })
      .withHubProtocol(new JsonHubProtocol())
      .withAutomaticReconnect()
      .build();

      await this.hubConnection
        .start()
        .then(() => console.log('SignalR connected'))
        .catch(err => console.error('Error while starting SignalR connection', err))
  }

  public async stopConnection(): Promise<void> {
    if (!this.hubConnection) return;

    this.hubConnection
      .stop()
      .then(() => console.log('SignalR disconnected'))
      .catch(err => console.error('Error while closing SignalR connection', err))
  }

  public subscribe<T extends BaseEvent>(ctor: new () => T, then: (event: T) => void): void {
    if (!this.hubConnection) return;

    const name = new ctor().className;

    this.hubConnection
      .on(name, (event: T) => then(event));

    console.log(`Subscribed to ${name} SignalR event`);
  }

  public unsubscribe<T extends BaseEvent>(ctor: new() => T): void {
    if (!this.hubConnection) return;

    const name = new ctor().className;

    this.hubConnection
      .off(name);

    console.log(`Unsubscribed from ${name} SignalR event`);
  }
}
