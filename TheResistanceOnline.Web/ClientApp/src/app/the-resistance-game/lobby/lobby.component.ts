import { Component, Input, OnInit } from '@angular/core';
import { GameDetails } from '../the-resistance-game.models';
import { TheResistanceGameService } from '../the-resistance-game.service';

@Component({
             selector: 'app-lobby',
             templateUrl: './lobby.component.html',
             styleUrls: ['./lobby.component.css']
           })
export class LobbyComponent implements OnInit {
  // @Input() gameDetails: GameDetails = {
  //   lobbyName: '',
  //   playersDetails: []
  // };

  constructor(private gameService: TheResistanceGameService) {

  }

  ngOnInit(): void {
    //this.gameService.addUserJoinedLobbyListener();
    //this.gameService.addUserLeftLobbyListener();
   // this.gameService.addUserLeftGameListener();

  }

  ngOnDestroy() {
    console.log('lobby destroyed');
  }
}
