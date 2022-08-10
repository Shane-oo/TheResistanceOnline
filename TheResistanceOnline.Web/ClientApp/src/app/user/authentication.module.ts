import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRegisterComponent } from './user-register/user-register.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { UserLoginComponent } from './user-login/user-login.component';
import { UserForgotPasswordComponent } from './user-forgot-password/user-forgot-password.component';
import { UserResetPasswordComponent } from './user-reset-password/user-reset-password.component';
import { UserEmailConfirmationComponent } from './user-email-confirmation/user-email-confirmation.component';


@NgModule({
            declarations: [UserRegisterComponent,
              UserLoginComponent,
              UserForgotPasswordComponent,
              UserResetPasswordComponent,
              UserEmailConfirmationComponent],
            exports: [
              UserRegisterComponent
            ],
            imports: [
              CommonModule,
              ReactiveFormsModule,
              RouterModule.forChild([
                                      {path: 'register', component: UserRegisterComponent},
                                      {path: 'login', component: UserLoginComponent},
                                      {path: 'forgot-password', component: UserForgotPasswordComponent},
                                      {path: 'reset-password', component: UserResetPasswordComponent},
                                      {path: 'email-confirmation', component: UserEmailConfirmationComponent}
                                    ])
            ]
          })
export class AuthenticationModule {}
