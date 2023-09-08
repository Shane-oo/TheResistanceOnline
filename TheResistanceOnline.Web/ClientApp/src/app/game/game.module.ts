import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";

import {GameComponent} from './game.component';
import {GameLobbyComponent} from './game-lobby/game-lobby.component';
import {GameLobbyLobbiesComponent} from './game-lobby/game-lobby-lobbies/game-lobby-lobbies.component';
import {GameLobbySearchComponent} from './game-lobby/game-lobby-search/game-lobby-search.component';
import {GameLobbyCreateComponent} from './game-lobby/game-lobby-create/game-lobby-create.component';
import {ReactiveFormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    GameComponent,
    GameLobbyComponent,
    GameLobbyLobbiesComponent,
    GameLobbySearchComponent,
    GameLobbyCreateComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: GameComponent}]),
    ReactiveFormsModule,
  ]
})
export class GameModule {
}
