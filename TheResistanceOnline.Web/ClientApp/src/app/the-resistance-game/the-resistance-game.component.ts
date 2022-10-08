import { Component, OnInit } from '@angular/core';
import { GameDetails } from './the-resistance-game.models';
import { TheResistanceGameService } from './the-resistance-game.service';

@Component({
             selector: 'app-the-resistance-game',
             templateUrl: './the-resistance-game.component.html',
             styleUrls: ['./the-resistance-game.component.css']
           })
export class TheResistanceGameComponent implements OnInit {
  public userIsInGame: boolean = false;
  public gameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false
  };

  constructor(private gameService: TheResistanceGameService) {

    this.gameService.gameDetailsChanged.subscribe((value: GameDetails) => {
      this.gameDetails = value;
      this.userIsInGame = true;
    });

    //

    this.gameService.addReceiveGameDetailsListener();
  }

  ngOnInit(): void {
  }

}
