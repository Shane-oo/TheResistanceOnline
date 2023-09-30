import {AfterViewInit, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {StartGameCommand} from "../game.models";
import * as signalR from "@microsoft/signalr";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {environment} from "../../../environments/environment";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";


@Component({
  selector: 'app-game-resistance',
  templateUrl: './game-resistance.component.html',
  styleUrls: ['./game-resistance.component.css']
})
export class GameResistanceComponent implements OnInit, OnDestroy, AfterViewInit {
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

  async ngAfterViewInit() {
  }

  async ngOnDestroy() {
    await this.stop();
  }

  private async start() {
    if (this.startGameCommand) {
      try {
        this.swalService.showSwal(SwalTypes.Info, "Connecting To Resistance Hub...");

        // add query params to connection request
        this.resistanceHubConnection.baseUrl += `?lobbyId=${this.startGameCommand.lobbyId}`;

        await this.resistanceHubConnection.start().then(() => {
          this.swalService.showSwal(SwalTypes.Success, "Connected To Resistance Hub");
          // my monstrosity of a hack to hopefully stop two people from sending startGame at the same time
          // so that commenceGame isnt done twice
          // do better shane
          setTimeout(() => this.startGame(), Math.floor(Math.random() * 9000))

          // add required initial listeners
        });
      } catch (err) {
        this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Resistance Hub');
      }
    }
  }

  private async stop() {
    await this.resistanceHubConnection.stop();
  }

  private startGame() {
    if (this.startGameCommand) {
      this.resistanceHubConnection.invoke("StartGame", this.startGameCommand)
        .then(() => {
          this.swalService.showSwal(SwalTypes.Info, "Waiting For Game To Start...")
        })
        .catch(err => {
        });
    }
  }


}
