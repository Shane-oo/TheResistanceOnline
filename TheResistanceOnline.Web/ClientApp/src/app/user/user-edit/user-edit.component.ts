import { Component, OnInit } from '@angular/core';
import { UserDetailsModel } from './user-edit.models';
import { UserEditService } from './user-edit.service';
import { AuthenticationService } from '../authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { GetUserCommandModel } from '../user.models';

@Component({
             selector: 'app-user-edit',
             templateUrl: './user-edit.component.html',
             styleUrls: ['./user-edit.component.css']
           })
export class UserEditComponent implements OnInit {
  public test: string = '';
  private _getUserCommand!: GetUserCommandModel;
  private _userDetails!:UserDetailsModel;
  private _userId: string = '';

  constructor(private userEditService: UserEditService, private authService: AuthenticationService) {
  }

  ngOnInit(): void {
    this._userId = this.authService.getUserId();

    this._getUserCommand = {userId: this._userId};

    this.userEditService.getUserDetails(this._getUserCommand).subscribe({
                                                                  next: (response: UserDetailsModel) => {
                                                                    this._userDetails = response;
                                                                  },
                                                                  error: (err: HttpErrorResponse) => {
                                                                    console.log(err);
                                                                  }
                                                                });
  }

}
