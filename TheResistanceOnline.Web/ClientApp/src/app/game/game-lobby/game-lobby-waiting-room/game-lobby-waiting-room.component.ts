import {Component, EventEmitter, Input, Output} from '@angular/core';
import {LobbyDetails, ReadyUpCommand} from "../game-lobby.models";

@Component({
  selector: 'app-game-lobby-waiting-room',
  templateUrl: './game-lobby-waiting-room.component.html',
  styleUrls: ['./game-lobby-waiting-room.component.css']
})
export class GameLobbyWaitingRoomComponent {
  @Input() lobby!: LobbyDetails;
  @Output() readyEventEmitter = new EventEmitter<ReadyUpCommand>();

  public ready() {
    const command: ReadyUpCommand = {
      lobbyId: this.lobby.id
    }
    this.readyEventEmitter.emit(command);
  }
}
