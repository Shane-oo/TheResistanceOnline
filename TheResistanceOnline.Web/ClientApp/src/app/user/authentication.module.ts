import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRegisterComponent } from './user-register/user-register.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
            declarations: [UserRegisterComponent],
            exports: [
              UserRegisterComponent
            ],
            imports: [
              CommonModule,
              ReactiveFormsModule,
              RouterModule.forChild([
                                      {path: 'register', component: UserRegisterComponent}
                                    ])
            ]
          })
export class AuthenticationModule {}
