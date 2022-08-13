import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { GameComponent } from './game/game.component';
import { GameCanvasComponent } from './game/game-canvas/game-canvas.component';
import { GameChatComponent } from './game/game-chat/game-chat.component';
import { ErrorHandlerService } from './shared/services/error-handler.service';

import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { SwalContainerComponent } from '../ui/swal/swal-container.component';
import { SwalContainerService } from '../ui/swal/swal-container.service';
import { OverlayComponent } from '../ui/overlay/overlay.component';
import { OverlayService } from '../ui/overlay/overlay.service';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthGuard } from './shared/guards/auth.guard';
import { AdminComponent } from './admin/admin.component';
import { AdminGuard } from './shared/guards/admin.guard';

export function tokenGetter() {
  return localStorage.getItem('TheResistanceToken');
}

@NgModule({
            declarations: [
              AppComponent,
              NavMenuComponent,
              HomeComponent,
              CounterComponent,
              FetchDataComponent,
              GameComponent,
              GameCanvasComponent,
              GameChatComponent,
              SwalContainerComponent,
              OverlayComponent,
              AdminComponent
            ],
            imports: [
              BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
              HttpClientModule,
              FormsModule,
              RouterModule.forRoot([
                                     {path: '', component: HomeComponent, pathMatch: 'full'},
                                     {path: 'counter', component: CounterComponent},
                                     {path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard]},
                                     {path: 'game', component: GameComponent, canActivate: [AuthGuard]},
                                     {
                                       path: 'user',
                                       loadChildren: () => import('./user/authentication.module').then(m => m.AuthenticationModule)
                                     },
                                     {path: 'admin', component: AdminComponent, canActivate: [AuthGuard, AdminGuard]},
                                     {
                                       path: 'user/user-edit',
                                       loadChildren: () => import('./user/user-edit/user-edit.module').then(m => m.UserEditModule),
                                       canActivate: [AuthGuard]
                                     }
                                   ]),
              SweetAlert2Module.forRoot(),
              JwtModule.forRoot({
                                  config: {
                                    tokenGetter: tokenGetter,
                                    allowedDomains: ['localhost:44452',
                                      'theresistanceboardgameonline.com',
                                      'localhost:7158',
                                      'localhost:5001'],
                                    disallowedRoutes: []
                                  }
                                })
            ],
            providers: [
              {
                provide: HTTP_INTERCEPTORS,
                useClass: ErrorHandlerService,
                multi: true
              },
              SwalContainerService,
              OverlayService],
            exports: [],
            bootstrap: [AppComponent]
          })
export class AppModule {}
