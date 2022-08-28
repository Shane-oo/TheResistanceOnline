import { Component, OnInit } from '@angular/core';
import { GameDetails } from './the-resistance-game.models';
import { TheResistanceGameService } from './the-resistance-game.service';

@Component({
             selector: 'app-the-resistance-game',
             templateUrl: './the-resistance-game.component.html',
             styleUrls: ['./the-resistance-game.component.css']
           })
export class TheResistanceGameComponent implements OnInit {
  public gameDetails: GameDetails = {
    userInGame: false,
    lobbyName: '',
    playersDetails: []
  };

  constructor(private gameService: TheResistanceGameService) {

    this.gameService.gameDetailsChanged.subscribe((value: GameDetails) => {
      this.gameDetails = value;
    });
  }

  ngOnInit(): void {
  }

  public changeUsersLobby = (lobbyName: string) => {

    this.gameDetails.userInGame = true;
    this.gameDetails.lobbyName = lobbyName;
  };
}
