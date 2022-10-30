import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { AuthenticationService } from './authentication.service';
import { UserSettingsUpdateCommand } from './user-settings.models';

@Injectable({
              providedIn: 'root'
            })
export class UserSettingsService {
  private readonly userSettingsEndpoint = '/api/UserSettings';

  // environment.API_URL
  constructor(private http: HttpClient, private router: Router, private authService: AuthenticationService) {
  }

  public updateUserSettings = (body: UserSettingsUpdateCommand) => {
    body.userId = this.authService.getUserId();
    console.log('user id got:',body.userId)
    return this.http.post(`${environment.API_URL}${this.userSettingsEndpoint}/UpdateUserSettings`, body);
  };
}
