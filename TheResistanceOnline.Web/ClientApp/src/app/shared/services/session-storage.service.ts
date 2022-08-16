import { Injectable } from '@angular/core';
import { UserDetailsModel } from '../../user/user-edit/user-edit.models';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../user/authentication.service';

@Injectable({
              providedIn: 'root'
            })
export class SessionStorageService {

  constructor(private authService: AuthenticationService) {
  }

  public saveUserDetails = (user: UserDetailsModel) => {
    sessionStorage.setItem('userDetails', JSON.stringify(user));
  };

  public getUserDetails = (): UserDetailsModel | undefined => {

    let userDetails = sessionStorage.getItem('userDetails');
    if(userDetails) {

      return JSON.parse(userDetails) as UserDetailsModel;
    } else {
      let userId = this.authService.getUserId();

      let getUserCommand = {userId: userId};

      this.authService.getUserDetails(getUserCommand).subscribe({
                                                                  next: (response: UserDetailsModel) => {
                                                                    this.saveUserDetails(response);
                                                                    return response;
                                                                  },
                                                                  error: (err: HttpErrorResponse) => {
                                                                    // ToDO swal something has gone wrong
                                                                  }
                                                                });
    }
    return;
  };

  public removeSessionStorage = () => {
    sessionStorage.clear();
  };
}
