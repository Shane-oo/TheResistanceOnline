import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { UserEditComponent } from './user-edit.component';

@NgModule({
            declarations: [UserEditComponent],
            exports: [],
            imports: [
              CommonModule,
              ReactiveFormsModule,
              RouterModule.forChild([
                                      {path: '', component: UserEditComponent}
                                    ])
            ]
          })
export class UserEditModule {
}
