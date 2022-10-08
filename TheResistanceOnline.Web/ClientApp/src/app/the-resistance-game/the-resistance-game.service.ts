import { Injectable } from '@angular/core';
import { GameDetails, JoinGameCommand } from './the-resistance-game.models';
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

  // public gameDetails: GameDetails = {
  //   playersDetails: [],
  //   lobbyName: ''
  // };

  public gameDetailsChanged: Subject<GameDetails> = new Subject<GameDetails>();

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

  private connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.Socket_URL + '/theresistancehub',
             this.options
    )
    .build();

  constructor(private http: HttpClient, private swalService: SwalContainerService, private userSettingsService: UserSettingsService) {
    this.connection.onclose(async() => {
      await this.start();
    });

    this.start().then(r => console.log('connected'));

    // todo Add Permanent Listeners

  }

  // start the connection
  public async start() {
    try {
      console.log('trying to connect');
      await this.connection.start();
    } catch(err) {
      console.log(err);
      setTimeout(() => this.start(), 30000);
    }
  }


  // join-game Listeners
  public addReceiveAllGameDetailsToPlayersNotInGameListener = () => {
    this.connection.on('ReceiveAllGameDetailsToPlayersNotInGame', (map: Map<string, GameDetails>) => {
      console.log('recivec all game details');
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

  public addReceiveGameDetailsListener = () => {
    this.connection.on('ReceiveGameDetails', (gameDetails: GameDetails) => {
      this.gameDetailsChanged.next(gameDetails);
    });
  };

  public addDiscordNotFoundListener = () => {
    this.connection.on('ReceiveDiscordNotFound', () => {
      console.log('discord not found triggered');
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
                                                                                                 'Successully declined Discord',
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
}


