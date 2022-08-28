import { Injectable } from '@angular/core';
import { CreateGameCommand, GameDetails } from './the-resistance-game.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({
              providedIn: 'root'
            })
export class TheResistanceGameService {
  public gameDetails: GameDetails = {
    userInGame: false,
    playersDetails: [],
    lobbyName: ''
  };

  public gameDetailsChanged: Subject<GameDetails> = new Subject<GameDetails>();

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

    this.start().then(r => console.log('connected'));

    this.gameDetailsChanged.subscribe((value) => {
      this.gameDetails = value;
    });
  }

  // start the connection
  public async start() {
    try {
      console.log('trying to connect');
      await this.connection.start();
    } catch(err) {
      console.log(err);
      setTimeout(() => this.start(), 5000);
    }
  }

  public createGame = (body: CreateGameCommand) => {
    console.log('invoking CreateGame()');
    this.connection.invoke('CreateGame', body)
        .catch(err => console.log(err));
  };


  public addUserCreatedGameListener = () => {
    this.connection.on('userCreatedGame', (newGame) => {
      console.log('User created new game', newGame);
      this.gameDetailsChanged.next(newGame);
    });
  };

}
