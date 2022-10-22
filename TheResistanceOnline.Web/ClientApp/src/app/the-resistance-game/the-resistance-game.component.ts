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

  public isTheHost: boolean = false;

  constructor(private gameService: TheResistanceGameService) {

    this.gameService.gameDetailsChanged.subscribe((value: GameDetails) => {
      this.gameDetails = value;
      this.userIsInGame = true;
    });

    this.gameService.hostChanged.subscribe((value: boolean) => {
      this.isTheHost = value;
    });

    this.gameService.addReceiveGameDetailsListener();
    this.gameService.addReceiveGameHostListener();
  }

  async ngOnInit() {
    // start hub connection
    await this.gameService.start().then(r => console.log('connected'));
  }

  async ngOnDestroy() {
    // stop hub connection
    await this.gameService.stop();
  }

}
