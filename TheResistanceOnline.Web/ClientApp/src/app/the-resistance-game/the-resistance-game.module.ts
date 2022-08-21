import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TheResistanceGameComponent } from './the-resistance-game.component';
import { GameComponent } from './game/game.component';
import { GameChatComponent } from './game/game-chat/game-chat.component';
import { GameCanvasComponent } from './game/game-canvas/game-canvas.component';
import { RouterModule } from '@angular/router';
import { LobbyComponent } from './lobby/lobby.component';


@NgModule({
            declarations: [
              TheResistanceGameComponent,
              GameComponent,
              GameChatComponent,
              GameCanvasComponent,
              LobbyComponent
            ],
            imports: [
              CommonModule,
              RouterModule.forChild([
                                      {path: '', component: TheResistanceGameComponent},
                                      {path: 'game', component: GameComponent}
                                    ])
            ]
          })
export class TheResistanceGameModule {}
