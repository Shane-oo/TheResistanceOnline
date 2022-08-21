import { Injectable } from '@angular/core';
import { CreateGameCommand } from './the-resistance-game.models';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
              providedIn: 'root'
            })
export class TheResistanceGameService {
  private readonly gamesEndpoint = '/api/Games';

  constructor(private http: HttpClient) {
  }
  public createGame = (body: CreateGameCommand) => {
    return this.http.post(`${environment.Socket_URL}${this.gamesEndpoint}/Create`, body);
  };
}
