import {Component, Input} from '@angular/core';
import {LobbyDetails} from "../../game-lobby.models";

@Component({
  selector: 'app-game-lobby-waiting-room-details',
  templateUrl: './game-lobby-waiting-room-details.component.html',
  styleUrls: ['./game-lobby-waiting-room-details.component.css']
})
export class GameLobbyWaitingRoomDetailsComponent {
  @Input() lobby!: LobbyDetails;
}
