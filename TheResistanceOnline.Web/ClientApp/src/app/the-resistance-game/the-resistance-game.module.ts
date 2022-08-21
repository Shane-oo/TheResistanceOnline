import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TheResistanceGameComponent } from './the-resistance-game.component';
import { GameComponent } from './game/game.component';
import { GameChatComponent } from './game/game-chat/game-chat.component';
import { GameCanvasComponent } from './game/game-canvas/game-canvas.component';
import { RouterModule } from '@angular/router';
import { CreateGameComponent } from './create-game/create-game.component';
import { JoinGameComponent } from './join-game/join-game.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
            declarations: [
              TheResistanceGameComponent,
              GameComponent,
              GameChatComponent,
              GameCanvasComponent,
              CreateGameComponent,
              JoinGameComponent
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
