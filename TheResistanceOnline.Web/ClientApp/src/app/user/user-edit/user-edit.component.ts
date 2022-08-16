import { Component, OnInit } from '@angular/core';
import { UserDetailsModel } from './user-edit.models';
import { UserEditService } from './user-edit.service';
import { AuthenticationService } from '../authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { GetUserCommandModel } from '../user.models';
import { SessionStorageService } from '../../shared/services/session-storage.service';

@Component({
             selector: 'app-user-edit',
             templateUrl: './user-edit.component.html',
             styleUrls: ['./user-edit.component.css']
           })
export class UserEditComponent implements OnInit {
  public test: string = '';
  public _userDetails: UserDetailsModel | undefined;
  private _getUserCommand!: GetUserCommandModel;
  private _userId: string = '';

  constructor(private userEditService: UserEditService, private authService: AuthenticationService, private sessionService: SessionStorageService) {
  }

  ngOnInit(): void {
    this._userDetails = this.sessionService.getUserDetails();
    console.log(this._userDetails);
  }

}
