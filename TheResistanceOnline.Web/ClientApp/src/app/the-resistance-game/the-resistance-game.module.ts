import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TheResistanceGameComponent } from './the-resistance-game.component';
import { GameComponent } from './game/game.component';
import { GameCanvasComponent } from './game/game-canvas/game-canvas.component';
import { RouterModule } from '@angular/router';
import { JoinGameComponent } from './join-game/join-game.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JoinGameDetailsComponent } from './join-game/join-game-details/join-game-details.component';
import { GameLobbyComponent } from './game-lobby/game-lobby.component';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CountdownModule } from 'ngx-countdown';

@NgModule({
            declarations: [
              TheResistanceGameComponent,
              GameComponent,
              GameCanvasComponent,
              JoinGameComponent,
              JoinGameDetailsComponent,
              GameLobbyComponent
            ],
            imports: [
              CommonModule,
              RouterModule.forChild([
                                      {path: '', component: TheResistanceGameComponent}]),
              ReactiveFormsModule,
              FormsModule,
              FontAwesomeModule,
              CountdownModule
            ]
          })
export class TheResistanceGameModule {}
