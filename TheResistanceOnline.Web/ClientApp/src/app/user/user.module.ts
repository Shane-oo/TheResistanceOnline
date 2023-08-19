import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';
import {UserLoginComponent} from './user-login/user-login.component';
import { UserEditComponent } from './user-edit/user-edit.component';

@NgModule({
    declarations: [
        UserLoginComponent,
        UserEditComponent
    ],
    exports: [],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule.forChild([
            {path: 'login', component: UserLoginComponent},
        ])
    ]
})
export class UserModule {
}
