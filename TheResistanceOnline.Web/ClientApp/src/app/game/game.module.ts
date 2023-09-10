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

@NgModule({
  declarations: [
    GameLobbyComponent,
    GameLobbyLobbiesComponent,
    GameLobbySearchComponent,
    GameLobbyCreateComponent,
    GameResistanceComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: 'lobby', component: GameLobbyComponent},
      {path: 'resistance', component: GameResistanceComponent}
    ]),

    ReactiveFormsModule,
    PipesModule,
  ]
})
export class GameModule {
}
