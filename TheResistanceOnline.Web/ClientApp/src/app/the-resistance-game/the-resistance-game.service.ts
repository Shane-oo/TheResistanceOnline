import { Injectable } from '@angular/core';
import { GameDetails, JoinGameCommand, StartGameCommand } from './the-resistance-game.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import * as signalR from '@microsoft/signalr';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { SwalContainerService, SwalTypesModel } from '../../ui/swal/swal-container.service';
import { UserSettingsService } from '../user/user-settings.service';
import { UserSettingsUpdateCommand } from '../user/user-settings.models';
import { DiscordLoginResponseModel } from '../user/user.models';

@Injectable({
              providedIn: 'root'
            })
export class TheResistanceGameService {

  public gameDetailsChanged: Subject<GameDetails> = new Subject<GameDetails>();

  public hostChanged: Subject<boolean> = new Subject<boolean>();

  //public allGameDetails
  public groupNameToGameDetailsMapChanged: Subject<Map<string, GameDetails>> = new Subject<Map<string, GameDetails>>();


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

  public connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.Socket_URL + '/theresistancehub',
             this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private http: HttpClient, private swalService: SwalContainerService, private userSettingsService: UserSettingsService) {
  }

  // start the connection
  public async start() {
    try {
      console.log('trying to connect');
      await this.connection.start();
      this.swalService.showSwal('Connected To Server', SwalTypesModel.Success);
    } catch(err) {
      console.log(err);
      this.swalService.showSwal('Error Connecting To Server', SwalTypesModel.Error);
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

  public joinGame = (body: JoinGameCommand) => {
    this.connection.invoke('ReceiveJoinGameCommand', body).then(() => {
    }).catch(err => console.log(err));
  };


  public addDiscordNotFoundListener = () => {
    this.connection.on('ReceiveDiscordNotFound', () => {
      this.swalService.fireDiscordLoginRequested();

      this.swalService.discordLoginResponseChanged.subscribe((value: DiscordLoginResponseModel) => {
        if(value.declinedLogin) {
          let userSettingsUpdateCommand: UserSettingsUpdateCommand = {
            updateUserWantsToUseDiscord: true,
            userWantsToUseDiscord: false
          };

          this.userSettingsService.updateUserSettings(userSettingsUpdateCommand).subscribe({
                                                                                             next: (response: any) => {
                                                                                               this.swalService.showSwal(
                                                                                                 'Successfully declined Discord',
                                                                                                 SwalTypesModel.Success);
                                                                                             }
                                                                                           });
        }
      });
    });
  };
  public removeDiscordNotFoundListener = () => {
    this.connection.off('ReceiveDiscordNotFound');
  };

  public addReceiveDiscordUserNotInDiscordServerListener = () => {
    this.connection.on('ReceiveDiscordUserNotInDiscordServer', () => {
    });
  };

  public removeReceiveDiscordUserNotInDiscordServerListener = () => {
    this.connection.off('ReceiveDiscordUserNotInDiscordServer');
  };

  // lobby listeners
  public addReceiveGameDetailsListener = () => {
    this.connection.on('ReceiveGameDetails', (gameDetails: GameDetails) => {
      this.gameDetailsChanged.next(gameDetails);
    });
  };
  public removeReceiveGameListener = () => {
    this.connection.off('ReceiveGameDetails');
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

  // this will probably needed to be done somewhere
  //https://code-maze.com/signalr-automatic-reconnect-option/
  //  this.hubConnection.onreconnected(() => {
  //   this.http.get('https://localhost:5001/api/chart')
  //   .subscribe(res => {
  //     console.log(res);
  //   })
  // })
}


