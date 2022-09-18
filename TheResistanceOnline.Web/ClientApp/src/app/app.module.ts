import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
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
import { environment } from '../environments/environment';

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
              SwalContainerComponent,
              OverlayComponent,
              AdminComponent
            ],
            imports: [
              BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
              BrowserAnimationsModule,
              HttpClientModule,
              FormsModule,
              RouterModule.forRoot([
                                     {path: '', component: HomeComponent, pathMatch: 'full'},
                                     {path: 'counter', component: CounterComponent},
                                     {path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard]},
                                     {
                                       path: 'user',
                                       loadChildren: () => import('./user/authentication.module').then(m => m.AuthenticationModule)
                                     },
                                     {path: 'admin', component: AdminComponent, canActivate: [AuthGuard, AdminGuard]},
                                     {
                                       path: 'user/user-edit',
                                       loadChildren: () => import('./user/user-edit/user-edit.module').then(m => m.UserEditModule),
                                       canActivate: [AuthGuard]
                                     },
                                     {
                                       path: 'the-resistance-game',
                                       loadChildren: () => import('./the-resistance-game/the-resistance-game.module').then(m => m.TheResistanceGameModule),
                                       canActivate: [AuthGuard]
                                     }
                                   ]),
              SweetAlert2Module.forRoot(),
              JwtModule.forRoot({
                                  config: {
                                    tokenGetter: tokenGetter,
                                    allowedDomains: [environment.API_Domain,
                                      environment.Socket_Domain,
                                      environment.Base_Domain,
                                      environment.Base_Domain + '/server'],
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
