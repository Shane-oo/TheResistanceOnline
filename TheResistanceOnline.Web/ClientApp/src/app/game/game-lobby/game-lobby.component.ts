import {Component, OnDestroy, OnInit} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {environment} from "../../../environments/environment";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {ConnectionModel, CreateLobbyCommand, JoinLobbyCommand, LobbyDetails, SearchLobbyQuery} from "../game.models";


@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.css']
})
export class GameLobbyComponent implements OnInit, OnDestroy {
  public currentLobby: LobbyDetails | null = null;
  public lobbies: LobbyDetails[] = [];
  private options: IHttpConnectionOptions = {
    accessTokenFactory: () => {
      const token = this.authService.authenticationData?.access_token;
      if (token) {
        return token;

      }
      return '';
    },
    transport: signalR.HttpTransportType.WebSockets,
    skipNegotiation: true,
    timeout: 120000 // 2 minutes
  };
  private connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.SERVER_URL + '/lobby',
      this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService) {
  }

  async ngOnInit() {

    // todo make a http call to web api so to refresh token before calling start

    await this.start();
  }

  async ngOnDestroy() {
    await this.stop();
  }

  public async start() {
    try {
      this.swalService.showSwal(SwalTypes.Info, "Connecting To Server...");

      await this.connection.start().then(() => {
        this.swalService.showSwal(SwalTypes.Success, 'Connected To Server');

        // add required initial listeners
        this.addReceiveErrorMessageListener();


        this.getLobbies();
      });
    } catch (err) {
      this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Server');
      setTimeout(() => this.start(), 30000);
    }
  }

  public async stop() {
    await this.connection.stop();
  }

  createLobby(command: CreateLobbyCommand) {
    this.connection.invoke('CreateLobby', command).then((lobbyDetails: LobbyDetails) => {
      this.setCurrentLobby(lobbyDetails);
    }).catch(err => {
      // do nothing with errors because of the error listener
    });
  };

  searchLobby(query: SearchLobbyQuery) {
    this.connection.invoke('SearchLobby', query).then((lobbyDetails: LobbyDetails) => {
      this.setCurrentLobby(lobbyDetails);
    }).catch(err => {

    });
  }

  joinLobby(command: JoinLobbyCommand) {
    this.connection.invoke('JoinLobby', command).then((lobbyDetails: LobbyDetails) => {
      this.setCurrentLobby(lobbyDetails);

    }).catch(err => {
    });
  }

  private setCurrentLobby = (lobbyDetails: LobbyDetails) => {
    this.currentLobby = lobbyDetails;

    this.stopReceiveNewLobbyListener();
    this.stopReceiveUpdatePublicLobbyListener();
    this.stopReceiveRemovePublicLobbyListener();

    this.addReceiveNewConnectionInLobbyListener();
    this.addReceiveLobbyClosedListener();
    this.addReceiveRemoveConnectionInLobbyListener();
  }

  private addReceiveNewLobbyListener = () => {
    this.connection.on("NewPublicLobby", (lobbyDetails: LobbyDetails) => {
      this.lobbies.push(lobbyDetails);
    });
  }

  private stopReceiveNewLobbyListener = () => {
    this.connection.off("NewPublicLobby");
  }

  private addReceiveNewConnectionInLobbyListener = () => {
    this.connection.on("NewConnectionInLobby", (newConnection: ConnectionModel) => {
      this.currentLobby!.connections.push(newConnection);
    });
  }

  private stopReceiveNewConnectionInLobbyListener = () => {
    this.connection.off("NewConnectionInLobby");
  }

  private addReceiveUpdatePublicLobbyListener = () => {
    this.connection.on('UpdatePublicLobby', (lobbyDetails: LobbyDetails) => {
      let lobbyToUpdate = this.lobbies.findIndex(l => l.id === lobbyDetails.id);
      if (lobbyToUpdate != -1) {
        this.lobbies[lobbyToUpdate] = lobbyDetails;
      }
    });
  }

  private stopReceiveUpdatePublicLobbyListener = () => {
    this.connection.off("UpdatePublicLobby");
  }

  private addReceiveRemovePublicLobbyListener = () => {
    this.connection.on('RemovePublicLobby', (lobbyId: string) => {
      this.lobbies = this.lobbies.filter(l => l.id !== lobbyId);
    });
  }

  private stopReceiveRemovePublicLobbyListener = () => {
    this.connection.off("RemovePublicLobby");
  }

  private addReceiveLobbyClosedListener = () => {
    this.connection.on('LobbyClosed', () => {
      this.currentLobby = null;

      this.getLobbies();
    });
  }

  private stopReceiveLobbyClosedListener = () => {
    this.connection.off('LobbyClosed');
  }

  private addReceiveRemoveConnectionInLobbyListener = () => {
    this.connection.on('RemoveConnectionInLobby', (connectionId: string) => {
      this.currentLobby!.connections = this.currentLobby!.connections.filter(c => c.connectionId !== connectionId);
    });
  }

  private stopReceiveRemoveConnectionInLobbyListener = () => {
    this.connection.off('RemoveConnectionInLobby');
  }

  private addReceiveErrorMessageListener = () => {
    this.connection.on("Error", (errorMessage: string) => {
      this.swalService.showSwal(SwalTypes.Error, errorMessage);
    });
  }

  private getLobbies() {
    this.connection.invoke("GetLobbies")
      .then((lobbies: LobbyDetails[]) => {
        this.lobbies = lobbies;
        // listen for new lobbies created/updated
        this.addReceiveNewLobbyListener();
        this.addReceiveUpdatePublicLobbyListener();
        this.addReceiveRemovePublicLobbyListener();
      });

  }

}
