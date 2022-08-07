import { Injectable } from '@angular/core';
import { UserLoginModel, UserLoginResponseModel, UserRegisterModel } from './user.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs';


@Injectable({
              providedIn: 'root'
            })
export class AuthenticationService {
  private readonly accountsEndpoint = '/api/Accounts';
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  // environment.API_URL
  constructor(private http: HttpClient) {
  }

  public registerUser = (body: UserRegisterModel) => {
    return this.http.post(`${environment.API_URL}${this.accountsEndpoint}/Register`, body);
  };

  public loginUser = (body: UserLoginModel) => {
    return this.http.post<UserLoginResponseModel>(`${environment.API_URL}${this.accountsEndpoint}/Login`, body);
  };

  public logout =()=>{
    localStorage.removeItem("TheResistanceToken");
    this.sendAuthStateChange(false);
  }
  public sendAuthStateChange = (isAuthenticated: boolean) => {

    this.authChangeSub.next(isAuthenticated);
  };
}
