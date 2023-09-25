import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {StartGameCommand} from "../game.models";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {environment} from "../../../environments/environment";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import * as signalR from "@microsoft/signalr";

@Component({
  selector: 'app-game-resistance',
  templateUrl: './game-resistance.component.html',
  styleUrls: ['./game-resistance.component.css']
})
export class GameResistanceComponent implements OnInit, OnDestroy {
  @Input() startGameCommand!: StartGameCommand;
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
    timeout: 120000, // 2 minutes
  };

  private resistanceHubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.SERVER_URL + '/resistance',
      this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();


  constructor(private authService: AuthenticationService, private swalService: SwalContainerService) {
  }

  async ngOnInit() {
    await this.start();
  }

  async ngOnDestroy() {
    await this.stop();
  }

  private async start() {
    if (this.startGameCommand) {
      try {
        this.swalService.showSwal(SwalTypes.Info, "Connection To Resistance Hub...");

        // add query params to connection request
        this.resistanceHubConnection.baseUrl += `?lobbyId=${this.startGameCommand.lobbyId}`;

        await this.resistanceHubConnection.start().then(() => {
          this.swalService.showSwal(SwalTypes.Success, "Connected To Resistance Hub");

          // todo send startGameCommand

          // add required initial listeners
        });
      } catch (err) {
        this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Resistance Hub');
        setTimeout(() => this.start(), 30000);
      }
    }
  }

  private async stop() {
    await this.resistanceHubConnection.stop();
  }
}
