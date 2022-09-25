import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { CreateDiscordUserCommand } from '../models/discord-server.models';
import { AuthenticationService } from '../../user/authentication.service';

@Injectable({
              providedIn: 'root'
            })
export class DiscordServerService {
  private readonly discordServersEndpoint = '/api/DiscordServers';

  // environment.API_URL
  constructor(private authService: AuthenticationService, private http: HttpClient, private router: Router) {
  }

  public createDiscordUser = (body: CreateDiscordUserCommand) => {
    body.userId = this.authService.getUserId();
    return this.http.post(`${environment.API_URL}${this.discordServersEndpoint}/CreateDiscordUser`, body);
  };
}
