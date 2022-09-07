import { Injectable } from '@angular/core';
import { CreateGameCommand, GameDetails, JoinGameCommand } from './the-resistance-game.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SwalContainerService, SwalTypesModel } from '../../ui/swal/swal-container.service';

@Injectable({
              providedIn: 'root'
            })
export class TheResistanceGameService {

  public gameDetails: GameDetails = {
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

  constructor(private http: HttpClient, private swalService: SwalContainerService) {
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

  public joinGame = (body: JoinGameCommand) => {
    this.connection.invoke('JoinGame', body).catch(err => console.log(err));
  };


  public addUserCreatedGameListener = () => {
    this.connection.on('userCreatedGame', (newGame: GameDetails) => {
      console.log('User created new game', newGame);
      this.gameDetailsChanged.next(newGame);
    });
  };

  public addGameAlreadyExistsListener = () => {
    this.connection.on('gameAlreadyExists', (response: string) => {
      this.swalService.showSwal(response, SwalTypesModel.Error);
    });
  };

  public addTooManyGamesListener = () => {
    this.connection.on('tooManyGames', (response: string) => {
      this.swalService.showSwal(response, SwalTypesModel.Error);
    });
  };


  public addUserJoinedGameListener = () => {
    this.connection.on('userJoinedGame', (game: GameDetails) => {
      console.log('user joined game', game);
      this.gameDetailsChanged.next(game);
    });

  };

  public addUserLeftGameListener = () => {
    this.connection.on('userLeftGame', (game: GameDetails) => {
      this.gameDetailsChanged.next(game);
    });
  };

  public addGameIsFullListener = () => {
    this.connection.on('gameIsFull', (response: string) => {
      this.swalService.showSwal(response, SwalTypesModel.Error);
    });
  };

  public addGameDoesNotExistListener = () => {
    this.connection.on('gameDoesNotExist', (response: string) => {
      this.swalService.showSwal(response, SwalTypesModel.Error);
    });
  };
}
