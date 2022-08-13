import { Injectable } from '@angular/core';
import { UserDetailsModel } from './user-edit.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';


@Injectable({
              providedIn: 'root'
            })
export class UserEditService {
  private readonly accountsEndpoint = '/api/Users';

  constructor(private http: HttpClient) {
  }

  public getUserDetails = (body: string) => {
    return this.http.post<UserDetailsModel>(`${environment.API_URL}${this.accountsEndpoint}/GetUser`, body);
  };
}


