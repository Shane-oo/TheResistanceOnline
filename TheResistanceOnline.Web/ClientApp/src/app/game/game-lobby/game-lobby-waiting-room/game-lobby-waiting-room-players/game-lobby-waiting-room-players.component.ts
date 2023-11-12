import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ConnectionModel} from "../../../game.models";

@Component({
  selector: 'app-game-lobby-waiting-room-players',
  templateUrl: './game-lobby-waiting-room-players.component.html',
  styleUrls: ['./game-lobby-waiting-room-players.component.css']
})
export class GameLobbyWaitingRoomPlayersComponent {
  @Input() connections: ConnectionModel[] = [];
  @Output() readyEventEmitter = new EventEmitter();

  public ready() {
    this.readyEventEmitter.emit();
  }
}
