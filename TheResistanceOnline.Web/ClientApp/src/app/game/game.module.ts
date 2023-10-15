import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {ReactiveFormsModule} from "@angular/forms";

import {PipesModule} from "../shared/pipes/pipes.module";

import {GameLobbyComponent} from './game-lobby/game-lobby.component';
import {GameLobbyLobbiesComponent} from './game-lobby/game-lobby-lobbies/game-lobby-lobbies.component';
import {GameLobbySearchComponent} from './game-lobby/game-lobby-search/game-lobby-search.component';
import {GameLobbyCreateComponent} from './game-lobby/game-lobby-create/game-lobby-create.component';
import {GameResistanceComponent} from './game-resistance/game-resistance.component';
import {GameStreamComponent} from './game-stream/game-stream.component';
import {GameStreamVideoComponent} from './game-stream/game-stream-video/game-stream-video.component';
import {GameLobbyWaitingRoomComponent} from './game-lobby/game-lobby-waiting-room/game-lobby-waiting-room.component';
import {
  GameLobbyWaitingRoomDetailsComponent
} from './game-lobby/game-lobby-waiting-room/game-lobby-waiting-room-details/game-lobby-waiting-room-details.component';
import {
  GameLobbyWaitingRoomPlayersComponent
} from './game-lobby/game-lobby-waiting-room/game-lobby-waiting-room-players/game-lobby-waiting-room-players.component';
import {GameComponent} from './game.component';
import {
  GameResistanceClassicComponent
} from './game-resistance/game-resistance-classic/game-resistance-classic.component';

@NgModule({
  declarations: [
    GameLobbyComponent,
    GameLobbyLobbiesComponent,
    GameLobbySearchComponent,
    GameLobbyCreateComponent,
    GameResistanceComponent,
    GameStreamComponent,
    GameStreamVideoComponent,
    GameLobbyWaitingRoomComponent,
    GameLobbyWaitingRoomDetailsComponent,
    GameLobbyWaitingRoomPlayersComponent,
    GameComponent,
    GameResistanceClassicComponent
  ],
  exports: [
    GameResistanceClassicComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: GameComponent},
      {path: 'game-dev', component: GameResistanceClassicComponent}
    ]),

    ReactiveFormsModule,
    PipesModule
  ]
})
export class GameModule {
}
