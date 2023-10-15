import {Component, EventEmitter, Input, Output} from '@angular/core';
import {JoinLobbyCommand, LobbyDetails} from "../game-lobby.models";

@Component({
  selector: 'app-game-lobby-lobbies',
  templateUrl: './game-lobby-lobbies.component.html',
  styleUrls: ['./game-lobby-lobbies.component.css']
})
export class GameLobbyLobbiesComponent {
  @Input() lobbies: LobbyDetails[] = [];
  @Output() joinLobbyEvent = new EventEmitter<JoinLobbyCommand>();

  public joinLobby(lobby: LobbyDetails) {
    if (lobby.connections.length < 10) {
      const joinLobbyCommand: JoinLobbyCommand = {
        lobbyId: lobby.id
      }
      this.joinLobbyEvent.emit(joinLobbyCommand);
    }
  }
}
