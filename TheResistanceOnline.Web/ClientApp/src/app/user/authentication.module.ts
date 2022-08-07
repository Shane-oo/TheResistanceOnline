import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRegisterComponent } from './user-register/user-register.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { UserLoginComponent } from './user-login/user-login.component';


@NgModule({
            declarations: [UserRegisterComponent, UserLoginComponent],
            exports: [
              UserRegisterComponent
            ],
            imports: [
              CommonModule,
              ReactiveFormsModule,
              RouterModule.forChild([
                                      {path: 'register', component: UserRegisterComponent},
                                      {path: 'login', component: UserLoginComponent}
                                    ])
            ]
          })
export class AuthenticationModule {}
