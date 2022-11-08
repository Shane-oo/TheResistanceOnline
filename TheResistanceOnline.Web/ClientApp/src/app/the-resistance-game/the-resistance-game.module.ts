import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TheResistanceGameComponent } from './the-resistance-game.component';
import { GameComponent } from './game/game.component';
import { GameChatComponent } from './game/game-chat/game-chat.component';
import { GameCanvasComponent } from './game/game-canvas/game-canvas.component';
import { RouterModule } from '@angular/router';
import { JoinGameComponent } from './join-game/join-game.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JoinGameDetailsComponent } from './join-game/join-game-details/join-game-details.component';
import { GameLobbyComponent } from './game-lobby/game-lobby.component';


@NgModule({
            declarations: [
              TheResistanceGameComponent,
              GameComponent,
              GameChatComponent,
              GameCanvasComponent,
              JoinGameComponent,
              JoinGameDetailsComponent,
              GameLobbyComponent
            ],
              imports: [
                  CommonModule,
                  RouterModule.forChild([
                                            {path: '', component: TheResistanceGameComponent},
                                            {path: 'game', component: GameComponent}
                                        ]),
                  ReactiveFormsModule,
                  FormsModule
              ]
          })
export class TheResistanceGameModule {}
