import {Component, OnInit} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {HttpError, IHttpConnectionOptions} from "@microsoft/signalr";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {environment} from "../../../environments/environment";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {HttpErrorResponse} from "@angular/common/http";
import {UpdateUserCommand, UserDetailsModel} from "../../user/user.models";
import {takeUntil} from "rxjs";
import {CreateLobbyCommand} from "../game.models";
import {JoinGameCommand} from "../../the-resistance-game/the-resistance-game.models";

@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.css']
})
export class GameLobbyComponent implements OnInit {

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
    timeout: 600000 // 10 minutes
  };
  public connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.SERVER_URL + '/lobby',
      this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService) {
  }

  async ngOnInit() {
    await this.start();
  }

  public async start() {
    try {
      await this.connection.start();
      this.swalService.showSwal(SwalTypes.Success, 'Connected To Server');
    } catch (err) {
      this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Server');
      setTimeout(() => this.start(), 30000);
    }
  }

  createLobby(command: CreateLobbyCommand) {
    this.connection.invoke('ReceiveCreateLobbyCommand', command).then(() => {
    }).catch(err => console.log(err));
  };

}
