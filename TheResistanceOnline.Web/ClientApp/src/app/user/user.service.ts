import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";

import {UpdateUserCommand, UserDetailsModel} from "./user.models";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly usersEndpoint = "/api/Users"

  constructor(private http: HttpClient) {
  }

  public getUserDetails() {
    return this.http.get<UserDetailsModel>(`${environment.API_URL}${this.usersEndpoint}`);
  };

  public updateUser(command: UpdateUserCommand) {
    return this.http.post<UserDetailsModel>(`${environment.API_URL}${this.usersEndpoint}`, command);
  }
}
