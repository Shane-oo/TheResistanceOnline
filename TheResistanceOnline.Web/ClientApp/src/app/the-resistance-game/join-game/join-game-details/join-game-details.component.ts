import { Component, Input, OnInit } from '@angular/core';
import { GameDetails, JoinGameCommand } from '../../the-resistance-game.models';
import { TheResistanceGameService } from '../../the-resistance-game.service';

@Component({
             selector: 'app-join-game-details',
             templateUrl: './join-game-details.component.html',
             styleUrls: ['./join-game-details.component.css']
           })
export class JoinGameDetailsComponent implements OnInit {

  @Input() selectedGameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false
  };

  constructor(private gameService: TheResistanceGameService) {
  }

  ngOnInit(): void {
  }


  public joinGame = (channelName: string) => {
    console.log('join ', channelName);
    const joinGameCommand: JoinGameCommand = {
      channelName: channelName
    };
    this.gameService.joinGame(joinGameCommand);
  };

}
