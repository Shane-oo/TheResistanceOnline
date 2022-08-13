import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../user/authentication.service';
import { Router } from '@angular/router';

@Injectable({
              providedIn: 'root'
            })
export class SessionStorageService {

  constructor(private authService: AuthenticationService, private router: Router) {
  }

  public saveUserId = (userId: string) => {
    sessionStorage.setItem('userId', userId);
  };

  public getUserId = (): string => {
    let userId = sessionStorage.getItem('userId');
    // user is not logged in send em back
    if(userId == null) {
      this.authService.sendAuthStateChange(false);
      this.router.navigate(['user/']);
      return '';
    }
    return userId;
  };

  public deleteData = () => {
    sessionStorage.clear();
  };
}
