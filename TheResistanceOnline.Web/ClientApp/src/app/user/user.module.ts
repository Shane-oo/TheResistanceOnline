import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';

import {authorizationGuard} from "../shared/guards/auth.guard";

import {UserLoginComponent} from './user-login/user-login.component';
import {UserDetailsComponent} from './user-details/user-details.component';
import {UserDetailsEditComponent} from './user-details/user-details-edit/user-details-edit.component';
import {UserDetailsFriendsComponent} from './user-details/user-details-friends/user-details-friends.component';
import {
  UserDetailsInteractionsComponent
} from './user-details/user-details-interactions/user-details-interactions.component';


@NgModule({
  declarations: [
    UserLoginComponent,
    UserDetailsComponent,
    UserDetailsEditComponent,
    UserDetailsFriendsComponent,
    UserDetailsInteractionsComponent
  ],
  exports: [],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {path: 'login', component: UserLoginComponent},
      {path: 'details', component: UserDetailsComponent, canActivate: [authorizationGuard]}
    ])
  ]
})
export class UserModule {
}
