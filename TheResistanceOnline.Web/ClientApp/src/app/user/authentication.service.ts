import { Injectable } from '@angular/core';
import { LoginResponseModel, UserForgotPasswordModel, UserLoginModel, UserRegisterModel, UserResetPasswordModel } from './user.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable({
              providedIn: 'root'
            })
export class AuthenticationService {
  private readonly accountsEndpoint = '/api/Accounts';
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  // environment.API_URL
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {
  }

  public registerUser = (body: UserRegisterModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/Register`, body);
  };

  public loginUser = (body: UserLoginModel) => {
    return this.http.post<LoginResponseModel>(`${environment.API_URL}${this.accountsEndpoint}/Login`, body);
  };

  public logout = () => {
    localStorage.removeItem('TheResistanceToken');
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
    return role === 'Administrator';
  };

  public sendUserForgotPassword = (body: UserForgotPasswordModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/ForgotPassword`, body);
  };

  public resetPassword = (body: UserResetPasswordModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/ResetPassword`, body);
  };
}