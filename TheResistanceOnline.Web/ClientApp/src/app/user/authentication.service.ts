import { Injectable } from '@angular/core';
import {
  LoginResponseModel,
  UserConfirmEmailModel,
  UserForgotPasswordModel,
  UserLoginModel,
  UserRegisterModel,
  UserResetPasswordModel
} from './user.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { UserDetailsModel } from './user-edit/user-edit.models';


@Injectable({
              providedIn: 'root'
            })
export class AuthenticationService {
  private readonly accountsEndpoint = '/api/Accounts';
  private readonly userEndpoint = '/api/User';
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  // environment.API_URL
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private router: Router) {
  }

  public registerUser = (body: UserRegisterModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/Register`, body);
  };

  public loginUser = (body: UserLoginModel) => {
    return this.http.post<LoginResponseModel>(`${environment.API_URL}${this.accountsEndpoint}/Login`, body);
  };

  public logout = () => {
    localStorage.removeItem('TheResistanceToken');
    localStorage.removeItem('TheResistanceUserId');
    this.sendAuthStateChange(false);
  };
  public sendAuthStateChange = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  };
  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem('TheResistanceToken');
    let isUserAuthenticated = false;
    if(token) {
      if(!this.jwtHelper.isTokenExpired(token)) {
        isUserAuthenticated = true;
      }

    }
    return isUserAuthenticated;
  };

  public isUserAdmin = (): boolean => {
    const token = localStorage.getItem('TheResistanceToken');
    let role = '';
    if(token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }

    return (role === 'Administrator');
  };

  public sendUserForgotPassword = (body: UserForgotPasswordModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/ForgotPassword`, body);
  };

  public resetPassword = (body: UserResetPasswordModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/ResetPassword`, body);
  };

  public confirmEmail = (body: UserConfirmEmailModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/ConfirmEmail`, body);
  };

  public getUserId = (): string => {
    const userId = localStorage.getItem('TheResistanceUserId');
    if(userId) {
      return userId;
    } else {
      // user is not logged in
      this.sendAuthStateChange(false);
      // Route to redirect url or homepage
      this.router.navigate([`/user/login`]).then(r => {
      });
    }
    return '';
  };

  public getUserDetails = () => {
    const id = this.getUserId();
    return this.http.get<UserDetailsModel>(`${environment.API_URL}${this.userEndpoint}/${id}`);
  };
}


// getJob(id: number): Observable<JobEditModel> {
//   return this.http.get<JobEditModel>(`${this.jobsEndpoint}/${id}`);
// }

//
// recalculatePrices(jobId: number): Observable<JobEditModel> {
//   return this.http.get<JobEditModel>(`${this.jobsEndpoint}/${jobId}/RecalculatePrices`);
// }
