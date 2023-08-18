import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {ErrorHandlerService} from './shared/services/error-handler.service';

import {SweetAlert2Module} from '@sweetalert2/ngx-sweetalert2';
import {SwalContainerComponent} from '../ui/swal/swal-container.component';
import {SwalContainerService} from '../ui/swal/swal-container.service';
import {OverlayComponent} from '../ui/overlay/overlay.component';
import {OverlayService} from '../ui/overlay/overlay.service';

import {authorizationGuard} from './shared/guards/auth.guard';
import {AdminComponent} from './admin/admin.component';
import {adminGuard} from './shared/guards/admin.guard';
import {HomeComponent} from './home/home.component';
import {AuthenticationInterceptor} from "./shared/services/authentication/authentication.interceptor";

export function tokenGetter() {
  return localStorage.getItem('TheResistanceToken');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    FetchDataComponent,
    SwalContainerComponent,
    OverlayComponent,
    AdminComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'counter', component: CounterComponent},
      {path: 'fetch-data', component: FetchDataComponent, canActivate: [authorizationGuard]},
      {
        path: 'user',
        loadChildren: () => import('./user/user.module').then(m => m.UserModule)
      },
      {path: 'admin', component: AdminComponent, canActivate: [authorizationGuard, adminGuard]},
      {
        path: 'the-resistance-game',
        loadChildren: () => import('./the-resistance-game/the-resistance-game.module').then(m => m.TheResistanceGameModule),
        canActivate: [authorizationGuard]
      }
    ], {initialNavigation: 'enabledBlocking'}),
    SweetAlert2Module.forRoot(),
    NgbModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlerService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    },

    SwalContainerService,
    OverlayService],
  exports: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
