import {Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {environment} from "../../../environments/environment";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {
  CreateLobbyCommand,
  JoinLobbyCommand,
  LobbyDetails,
  ReadyUpCommand,
} from "./game-lobby.models";
import {ConnectionModel, StartGameModel} from "../game.models";


@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.css']
})
export class GameLobbyComponent implements OnInit, OnDestroy {
  @Output() public startGameEvent: EventEmitter<StartGameModel> = new EventEmitter<StartGameModel>;
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
  private lobbyHubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
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
      this.swalService.showSwal(SwalTypes.Info, "Connecting To Lobby Hub...");

      await this.lobbyHubConnection.start().then(() => {
        this.swalService.showSwal(SwalTypes.Success, 'Connected To Lobby Hub');

        // add required initial listeners
        this.addReceiveErrorMessageListener();


        this.getLobbies();
      });
    } catch (err) {
      this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Server');
    }
  }

  public async stop() {
    await this.lobbyHubConnection.stop();
  }

  readyUp(command: ReadyUpCommand) {
    this.lobbyHubConnection.invoke('ReadyUp', command).catch(err => {
    });
  }

  createLobby(command: CreateLobbyCommand) {
    this.lobbyHubConnection.invoke('CreateLobby', command).then((lobbyDetails: LobbyDetails) => {
      this.setCurrentLobby(lobbyDetails);
    }).catch(err => {
      // do nothing with errors because of the error listener
    });
  };


  joinLobby(command: JoinLobbyCommand) {
    this.lobbyHubConnection.invoke('JoinLobby', command).then((lobbyDetails: LobbyDetails) => {
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
    this.addReceiveUpdateConnectionsReadyInLobbyListener();
    this.addReceiveStartGameListener();
  }

  private addReceiveNewLobbyListener = () => {
    this.lobbyHubConnection.on("NewPublicLobby", (lobbyDetails: LobbyDetails) => {
      this.lobbies.push(lobbyDetails);
    });
  }

  private stopReceiveNewLobbyListener = () => {
    this.lobbyHubConnection.off("NewPublicLobby");
  }

  private addReceiveNewConnectionInLobbyListener = () => {
    this.lobbyHubConnection.on("NewConnectionInLobby", (newConnection: ConnectionModel) => {
      if (this.currentLobby?.connections && newConnection) {
        this.currentLobby.connections.push(newConnection);
      }
    });
  }

  private stopReceiveNewConnectionInLobbyListener = () => {
    this.lobbyHubConnection.off("NewConnectionInLobby");
  }

  private addReceiveUpdatePublicLobbyListener = () => {
    this.lobbyHubConnection.on('UpdatePublicLobby', (lobbyDetails: LobbyDetails) => {
      let lobbyToUpdate = this.lobbies.findIndex(l => l.id === lobbyDetails.id);
      if (lobbyToUpdate != -1) {
        this.lobbies[lobbyToUpdate] = lobbyDetails;
      }
    });
  }

  private stopReceiveUpdatePublicLobbyListener = () => {
    this.lobbyHubConnection.off("UpdatePublicLobby");
  }

  private addReceiveRemovePublicLobbyListener = () => {
    this.lobbyHubConnection.on('RemovePublicLobby', (lobbyId: string) => {
      this.lobbies = this.lobbies.filter(l => l.id !== lobbyId);
    });
  }

  private stopReceiveRemovePublicLobbyListener = () => {
    this.lobbyHubConnection.off("RemovePublicLobby");
  }

  private addReceiveLobbyClosedListener = () => {
    this.lobbyHubConnection.on('LobbyClosed', () => {
      // best solution i found otherwise cause bugs
      this.swalService.showSwal(SwalTypes.Warning, "Host Closed The Lobby");
      location.reload();

    });
  }

  private stopReceiveLobbyClosedListener = () => {
    this.lobbyHubConnection.off('LobbyClosed');
  }

  private addReceiveRemoveConnectionInLobbyListener = () => {
    this.lobbyHubConnection.on('RemoveConnectionInLobby', (connectionId: string) => {
      this.currentLobby!.connections = this.currentLobby!.connections.filter(c => c.connectionId !== connectionId);
    });
  }

  private stopReceiveRemoveConnectionInLobbyListener = () => {
    this.lobbyHubConnection.off('RemoveConnectionInLobby');
  }

  private addReceiveUpdateConnectionsReadyInLobbyListener = () => {
    this.lobbyHubConnection.on("UpdateConnectionsReadyInLobby", (connectionId: string) => {
      const connectionToUpdateIndex = this.currentLobby!.connections.findIndex(c => c.connectionId === connectionId);
      if (connectionToUpdateIndex !== -1) {
        this.currentLobby!.connections[connectionToUpdateIndex].isReady = true;
      }
    });
  }

  private addReceiveStartGameListener = () => {
    this.lobbyHubConnection.on('StartGame', (startGameModel: StartGameModel) => {
      this.startGameEvent.emit(startGameModel);
    });
  }


  private addReceiveErrorMessageListener = () => {
    this.lobbyHubConnection.on("Error", (errorMessage: string) => {
      this.swalService.showSwal(SwalTypes.Error, errorMessage);
    });
  }

  private getLobbies() {
    this.lobbyHubConnection.invoke("GetLobbies")
      .then((lobbies: LobbyDetails[]) => {
        this.lobbies = lobbies;
        // listen for new lobbies created/updated
        this.addReceiveNewLobbyListener();
        this.addReceiveUpdatePublicLobbyListener();
        this.addReceiveRemovePublicLobbyListener();
      });

  }

}
