import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { MessageModel } from './game.models';
import { environment } from '../../environments/environment';

@Injectable({
              providedIn: 'root'
            })
export class GameService {
  public messages: MessageModel[] = [];
  public broadCastedMessages: MessageModel[] = [];
  private hubConnection!: signalR.HubConnection;

  constructor() {
  }

  public startConnection = () => {
    console.log('starting connection');
    console.log(environment.Socket_URL);
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.Socket_URL + '/message', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,

      })
      .build();

    this.hubConnection.start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err));
  };

  public addTransferMessageListener = () => {
    this.hubConnection.on('transfermessagedata', (data) => {
      this.messages = data;
      console.log(data);
    });
  };

  public broadCastMessage = (sentMessage: string) => {
    this.hubConnection.invoke('broadcastmessagedata', sentMessage)
        .catch(err => console.log(err));
    console.log('sent message ' + sentMessage);
  };

  public addBroadCastListener = () => {
    this.hubConnection.on('broadcastmessagedata', (data) => {
      this.broadCastedMessages = data;
      console.log(data);
    });
  };
}
