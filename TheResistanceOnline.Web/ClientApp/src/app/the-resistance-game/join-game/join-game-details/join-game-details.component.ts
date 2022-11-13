import { Component, Input, OnInit } from '@angular/core';
import { GameDetails, GameStage, JoinGameCommand } from '../../the-resistance-game.models';
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
    isAvailable: false,
    missionRound: 0,
    missionTeam: [],
    missionSize: 0,
    gameStage: GameStage.GameStart,
    gameOptions: {timeLimitMinutes: 0, moveTimeLimitMinutes: 0, botCount: 0}
  };

  constructor(private gameService: TheResistanceGameService) {
  }

  ngOnInit(): void {
  }


  public joinGame = (channelName: string) => {
    const joinGameCommand: JoinGameCommand = {
      channelName: channelName
    };
    this.gameService.joinGame(joinGameCommand);
  };

}
