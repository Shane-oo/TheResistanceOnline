import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRegisterComponent } from './user-register/user-register.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { UserLoginComponent } from './user-login/user-login.component';
import { UserForgotPasswordComponent } from './user-forgot-password/user-forgot-password.component';


@NgModule({
            declarations: [UserRegisterComponent, UserLoginComponent, UserForgotPasswordComponent],
            exports: [
              UserRegisterComponent
            ],
            imports: [
              CommonModule,
              ReactiveFormsModule,
              RouterModule.forChild([
                                      {path: 'register', component: UserRegisterComponent},
                                      {path: 'login', component: UserLoginComponent},
                                      {path: 'forgot-password', component: UserForgotPasswordComponent}
                                    ])
            ]
          })
export class AuthenticationModule {}
