import { Injectable } from '@angular/core';
import { UserDetailsModel } from './user-edit.models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { GetUserCommandModel } from '../user.models';


@Injectable({
              providedIn: 'root'
            })
export class UserEditService {
  private readonly userEndpoint = '/api/User';
  private readonly httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  };

  constructor(private http: HttpClient) {
  }


}


