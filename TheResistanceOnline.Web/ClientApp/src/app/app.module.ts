import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { GameComponent } from './game/game.component';
import { GameCanvasComponent } from './game/game-canvas/game-canvas.component';
import { GameChatComponent } from './game/game-chat/game-chat.component';

@NgModule({
            declarations: [
              AppComponent,
              NavMenuComponent,
              HomeComponent,
              CounterComponent,
              FetchDataComponent,
              GameComponent,
              GameCanvasComponent,
              GameChatComponent
            ],
            imports: [
              BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
              HttpClientModule,
              FormsModule,
              RouterModule.forRoot([
                                     {path: '', component: HomeComponent, pathMatch: 'full'},
                                     {path: 'counter', component: CounterComponent},
                                     {path: 'fetch-data', component: FetchDataComponent},
                                     {path: 'game', component: GameComponent},
                                     {
                                       path: 'user',
                                       loadChildren: () => import('./user/authentication.module').then(m => m.AuthenticationModule)
                                     }
                                   ])
            ],
            providers: [],
            bootstrap: [AppComponent]
          })
export class AppModule {}
