import { Injectable } from '@angular/core';
import{UserRegisterModel} from './user.models';
                        import{HttpClient} from '@angular/common/http';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly accountsEndpoint = '/api/Accounts';

 // environment.API_URL
  constructor(private http:HttpClient ) { }

  public registerUser = (body:UserRegisterModel)=>{
    return this.http.post<UserRegisterModel>(`${environment.API_URL}${this.accountsEndpoint}/Register`,body);
  }
}
