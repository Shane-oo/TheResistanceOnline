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
import { GameChatBoxComponent } from './game/game-chat-box/game-chat-box.component';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CountdownModule } from 'ngx-countdown';
import { NgbDropdown, NgbDropdownItem, NgbDropdownMenu, NgbDropdownToggle } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
            declarations: [
              TheResistanceGameComponent,
              GameComponent,
              GameCanvasComponent,
              JoinGameComponent,
              JoinGameDetailsComponent,
              GameLobbyComponent,
              GameChatBoxComponent
            ],
            imports: [
              CommonModule,
              RouterModule.forChild([
                                      {path: '', component: TheResistanceGameComponent}]),
              ReactiveFormsModule,
              FormsModule,
              FontAwesomeModule,
              CountdownModule,
              NgbDropdown,
              NgbDropdownMenu,
              NgbDropdownItem,
              NgbDropdownToggle
            ]
          })
export class TheResistanceGameModule {}
