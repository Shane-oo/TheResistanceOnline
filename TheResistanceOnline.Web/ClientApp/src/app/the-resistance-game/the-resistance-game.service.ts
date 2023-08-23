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

      let token = 'eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZDQkMtSFM1MTIiLCJraWQiOiJERTE1MkZDOThDQzlDN0VGRkU2RkVGMkEwNkFFRjEzMjY1QzJFQTU5IiwidHlwIjoiYXQrand0IiwiY3R5IjoiSldUIn0.hSik6WIpo7R8ldJyvXuca5tSDfvu65i6JC_tKG5lllAr_GVkiNgz44VxcxAmWSgCXKv3NKuk8PZl7LLYVhZG67FLjBZNDpjfBtH-vNLCdqdUrphXvjVlTtmGktxN_DwkjNmVvQSLS3ZjRwU7pkMiNkUJfiSgtg5_m2YqEHXOU6kcWJgwoa5bd71B0IxX-oHJPehupnn6R-0E03PWTEj425_uV4llnJJ0dyORPY-2pFVKhq3l6YCc7w8_7dvFHDX2TegHWXN4imk7583VQRE8qEXU2qN5p9LmqDBDYzXdcB0YPPzY3vw7KrHPCAoeXsxNIdz9f3aSz-gDEmozusrOAQ.PMTiz3nTmIsANhScn_y5kw.2G187ZJPOyiA8mdpy_X0_9gNhmPQJRcDIwoiWKks7BJRQT3oJ7S6PiplDVKL5DxPzBEm40xVP5HZ0Qex4OiqHF4eDP7IkaOQd2aEggeHVNGGbttzVtnRn7OgmKaOjKyHmH1hrVtnbIDQ3TpPnf_S7_5uGPg746fXOow00mSITAWFUsaW8sSO7btbtsW69fzZRjRg_n6ycWlSL38dG1OonKKqRE0B9ehFNFKR0nC2VJ2XzQbIPJb2mult4ovzjeQrRABT6I844I5p0UXZGUfmW7kzUCsytmQ5LfiFSUiEWyufNTMFgnJCjIrnbNdkAD7dw6AoSTNtP82EXKmYatSGhx2CxjMgaLbKAo7B-HdFU8GmO015lHppMAN4FRN6LdnpU4XgRuohufuIMweQUjHa8OrWWfe84nssutkCjbsfDJt0YNhVzLIDGtXv8j8_Z0kEE91d32_nZzZvsVNSQ1HLMykU8sCqo3E6Fsmq5RTcjlqvsvfjiNS7Oa-kBBaUlAip99uvnkp0p1tX0wDtyj4kju2AE9VgLVnxWKt6nzpHStse9u8feNeR5ejda5TE_9pyDeYJ2-9JHB8N-IaFp_YWPxKbUCxKZZzQNEA1Y-6f45TB9Oye33geqmDa_py8r-u-ZkDQv9GE9Bu86Uf86L49GT9RM9a0W5hdo-X8lriWRXIVCV7b7IUFGHuIoNsz7dxH3pWcOWTxpXMR0o1ESjVd1Lf72vHxlOJPzVETdBiYjACJ6c6xoSk3eTT2G2nbnKmxD_Xj75bgiEf14wgMRV5ZT6kW0ocKQnYD384plU5vEqU6qxcspk8DZN90vWbb_7xpAXrWxddAlnzeEQ8L0vXDOVymfQzTlKk5IvRbta67YDZL7sGpLi7_Zx-D0TZojyTAuZF0xSOmUSy_puICkBV-OQjqRi_1Bso73BHNKkeC9WCQyE_26qr2roFxluor4WsE1xTIR5MuetePdTP0Fa3CHL1YDvJaeJqeEAb1ZXB6SJlePHHGtyn0Gi5fs3D0Qsgl5ed8WoPMJ9J5K_eFd9bdNaGsxjKItAs0ynxp3vmjzjt08ey8Lk8L2DG5FXE2q2xhPjb8YNDTf7pwGuO4zPD6WpDZo298bMhTe3d5DahdhtlGjt6EdvisgB7sypMySHSkCLpiBTLwWkIIfaGcsfrHGBwdBEgC-2v-eiX6SJB9u6bYuZuCIWF3hwu8sjaiQrMTfYIrDkwQV7HzPEyQoAGARsfbuiiwi2cdpYIvjZYm10FTGguHZBNpikX9_tehbz6v.cZQkCfSh6P7kQD_kUYYNVuFz4WjJ9UsYvW5KTerAZeI';
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
    .withUrl(environment.SERVER_URL + '/lobby',
             this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private http: HttpClient, private swalService: SwalContainerService) {
  }

  // start the connection
  public async start() {
    try {
      console.log('trying to connect',this.options);
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


