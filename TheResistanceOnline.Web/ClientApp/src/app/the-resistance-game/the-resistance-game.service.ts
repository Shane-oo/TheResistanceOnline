import { Injectable } from '@angular/core';
import { GameActionCommand, GameDetails, JoinGameCommand, Message, StartGameCommand } from './the-resistance-game.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SwalContainerService, SwalTypes } from '../../ui/swal/swal-container.service';


@Injectable({
              providedIn: 'root'
            })
export class TheResistanceGameService {

  public gameDetailsChanged: Subject<GameDetails> = new Subject<GameDetails>();
  public playerIdChanged: Subject<string> = new Subject<string>();
  public hostChanged: Subject<boolean> = new Subject<boolean>();
  public groupNameToGameDetailsMapChanged: Subject<Map<string, GameDetails>> = new Subject<Map<string, GameDetails>>();

  public messageReceived: Subject<Message> = new Subject<Message>();


  private readonly gamesEndpoint = '/api/Games';

  private options: IHttpConnectionOptions = {
    accessTokenFactory: () => {
      let token = localStorage.getItem('TheResistanceToken');
      if(token) {
        return token;
      }
      return '';
    },
    transport: signalR.HttpTransportType.WebSockets,
    skipNegotiation: true,
    timeout: 600000 // 10 minutes
  };

  public connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.Socket_URL + '/theresistancehub',
             this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private http: HttpClient, private swalService: SwalContainerService) {
  }

  // start the connection
  public async start() {
    try {
      console.log('trying to connect');
      await this.connection.start();
      this.swalService.showSwal( SwalTypes.Success,'Connected To Server');
    } catch(err) {
      console.log(err);
      this.swalService.showSwal( SwalTypes.Error,'Error Connecting To Server');
      setTimeout(() => this.start(), 30000);
    }
  }

  public async stop() {
    try {
      await this.connection.stop();

    } catch(err) {
      console.log('error occured while disconnection');
      console.log(err);
    }
  }


  // join-game Listeners
  public addReceiveAllGameDetailsToPlayersNotInGameListener = () => {
    this.connection.on('ReceiveAllGameDetailsToPlayersNotInGame', (map: Map<string, GameDetails>) => {
      this.groupNameToGameDetailsMapChanged.next(map);
    });
  };

  public removeReceiveAllGameDetailsToPlayersNotInGameListener = () => {
    this.connection.off('ReceiveAllGameDetailsToPlayersNotInGame');
  };

  public addReceivePlayerId = () => {
    this.connection.on('ReceivePlayerId', (playerId: string) => {
      this.playerIdChanged.next(playerId);
    });

  };

  public removeReceivePlayerId = () => {
    this.connection.off('ReceivePlayerId');
  };

  public joinGame = (body: JoinGameCommand) => {
    this.connection.invoke('ReceiveJoinGameCommand', body).then(() => {
    }).catch(err => console.log(err));
  };

  // lobby listeners
  public addReceiveGameDetailsListener = () => {
    this.connection.on('ReceiveGameDetails', (gameDetails: GameDetails) => {
      this.gameDetailsChanged.next(gameDetails);
    });
  };

  public sendMessage = (body: string) => {
    this.connection.invoke('ReceiveMessageCommand', body).then(() => {
    }).catch(err => console.log(err));
  };
  public removeReceiveGameListener = () => {
    this.connection.off('ReceiveGameDetails');
  };

  public addReceiveMessageListener = () => {
    this.connection.on('ReceiveMessage', (message: Message) => {
      this.messageReceived.next(message);
    });
  };

  public removeReceiveMessageListener = () => {
    this.connection.off('ReceiveMessage');
  };

  public addReceiveGameHostListener = () => {
    this.connection.on('ReceiveGameHost', (isTheHost: boolean) => {
      this.hostChanged.next(isTheHost);
    });
  };

  public removeReceiveGameHostListener = () => {
    this.connection.off('ReceiveGameHost');
  };

  public startGame = (body: StartGameCommand) => {
    this.connection.invoke('ReceiveStartGameCommand', body).then(() => {
    }).catch(err => console.log(err));
  };

  public addReceiveGameFinishedListener = () => {
    this.connection.on('ReceiveGameFinished', () => {
      location.reload();
    });
  };


  public sendGameActionCommand = (gameActionCommand: GameActionCommand) => {
    this.connection.invoke('ReceiveGameActionCommand', gameActionCommand).catch(err => console.log(err));
  };


  // this will probably needed to be done somewhere
  //https://code-maze.com/signalr-automatic-reconnect-option/
  //  this.hubConnection.onreconnected(() => {
  //   this.http.get('https://localhost:5001/api/chart')
  //   .subscribe(res => {
  //     console.log(res);
  //   })
  // })
}


