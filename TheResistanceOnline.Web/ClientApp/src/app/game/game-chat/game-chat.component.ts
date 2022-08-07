import { Component, OnInit } from '@angular/core';
import { GameService } from '../game.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
             selector: 'app-game-chat',
             templateUrl: './game-chat.component.html',
             styleUrls: ['./game-chat.component.css']
           })
export class GameChatComponent implements OnInit {

  constructor(public _gameService: GameService, private http: HttpClient) {
  }

  ngOnInit(): void {
    this._gameService.startConnection();
    this._gameService.addTransferMessageListener();
    this._gameService.addBroadCastListener();
    this.startHttpRequest();
  }

  public onClickSubmit(data: any) {
    console.log('trying to broadcast: ', data);
    this._gameService.broadCastMessage(data.message.value);
  }

  private startHttpRequest = () => {
    console.log('start http request sending to:');
    console.log(environment.Socket_URL);
    this.http.get(environment.Socket_URL + '/api/messages')
        .subscribe(res => {
          console.log(res);
        });
  };
}
