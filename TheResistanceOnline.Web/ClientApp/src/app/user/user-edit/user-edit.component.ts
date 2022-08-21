import { Component, OnInit } from '@angular/core';
import { UserDetailsModel } from './user-edit.models';
import { UserEditService } from './user-edit.service';
import { AuthenticationService } from '../authentication.service';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
             selector: 'app-user-edit',
             templateUrl: './user-edit.component.html',
             styleUrls: ['./user-edit.component.css']
           })
export class UserEditComponent implements OnInit {
  public test: string = '';
  public _userDetails: UserDetailsModel | undefined;


  constructor(private userEditService: UserEditService, private authService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.authService.getUserDetails().subscribe({
                                                  next: (response: UserDetailsModel) => {
                                                    this._userDetails = response;
                                                  },
                                                  error: (err: HttpErrorResponse) => {
                                                    console.log(err);
                                                  }
                                                });
  }

}
