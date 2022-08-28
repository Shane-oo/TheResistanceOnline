import { Injectable } from '@angular/core';
import { CreateGameCommand } from './the-resistance-game.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { IHttpConnectionOptions } from '@microsoft/signalr';

@Injectable({
              providedIn: 'root'
            })
export class TheResistanceGameService {

  private readonly gamesEndpoint = '/api/Games';
  private readonly token = localStorage.getItem('TheResistanceToken');
  private options: IHttpConnectionOptions = {
    accessTokenFactory: () => {
      let token = localStorage.getItem('TheResistanceToken');
      if(token) {
        return token;
      }
      return '';
    }, transport: signalR.HttpTransportType.WebSockets, skipNegotiation: true

  };

  private connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.Socket_URL + '/theresistancehub',
             this.options
    )
    .build();

  constructor(private http: HttpClient) {
    this.connection.onclose(async() => {
      await this.start();
    });

    this.start().then(r =>console.log('connected'));
  }

  public createGame = (body: CreateGameCommand) => {
    console.log("1")
    this.connection.invoke('JoinGroup', body.lobbyName)
        .catch(err => console.log(err));

   // this.connection.invoke('broadcastmessagedata', body.lobbyName)
       // .catch(err => console.log(err));

    console.log('Successfully joined group ' +  body.lobbyName);
    console.log("2")
    //return this.http.post(`${environment.Socket_URL}${this.gamesEndpoint}/Create`, body);
  };

  // start the connection
  public async start() {
    try {
      console.log("trying to connect")
      await this.connection.start();
    } catch(err) {
      console.log(err);
      setTimeout(() => this.start(), 5000);
    }
  }

  // public startConnection = () => {
  //   console.log('starting connection');
  //   this.hubConnection = new signalR.HubConnectionBuilder()
  //     .withUrl(environment.Socket_URL + '/message', {
  //       skipNegotiation: true,
  //       transport: signalR.HttpTransportType.WebSockets
  //
  //     })
  //     .build();
  //
  //   this.hubConnection.start()
  //       .then(() => console.log('Connection started'))
  //       .catch(err => console.log('Error while starting connection: ' + err));
  // };
  //
  // public addTransferMessageListener = () => {
  //   this.hubConnection.on('transfermessagedata', (data) => {
  //     this.messages = data;
  //     console.log(data);
  //   });
  // };
  //
  // public broadCastMessage = (sentMessage: string) => {
  //   this.hubConnection.invoke('broadcastmessagedata', sentMessage)
  //       .catch(err => console.log(err));
  //   console.log('sent message ' + sentMessage);
  // };
  //
  // public addBroadCastListener = () => {
  //   this.hubConnection.on('broadcastmessagedata', (data) => {
  //     this.broadCastedMessages = data;
  //     console.log(data);
  //   });
  // };

}
