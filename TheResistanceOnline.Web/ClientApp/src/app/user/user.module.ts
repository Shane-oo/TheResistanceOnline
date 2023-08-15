import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';
import {UserLoginComponent} from './user-login/user-login.component';

@NgModule({
    declarations: [
        UserLoginComponent
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
