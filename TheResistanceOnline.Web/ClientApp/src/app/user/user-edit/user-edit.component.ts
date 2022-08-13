import { Component, OnInit } from '@angular/core';
import { UserDetailsModel } from './user-edit.models';
import { UserEditService } from './user-edit.service';
import { AuthenticationService } from '../authentication.service';
import { SessionStorageService } from '../../shared/services/session-storage.service';
import { LoginResponseModel } from '../user.models';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
   private _userDetails!:  UserDetailsModel;
   private _userId:string = "";
   public test:string = "";
  constructor(private userEditService:UserEditService,private sessionService:SessionStorageService) { }

  ngOnInit(): void {
     this._userId = this.sessionService.getUserId();

    this.userEditService.getUserDetails(this._userId).subscribe({
                                                 next: (response: UserDetailsModel) => {
                                                   this._userDetails = response;
                                                 },
                                                 error: (err: HttpErrorResponse) => {
                                                   console.log(err);
                                                 }
                                               });
  }

}
