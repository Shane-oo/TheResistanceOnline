import { Component, Input, OnInit } from '@angular/core';
import { GameAction, GameDetails, GameStage, JoinGameCommand } from '../../the-resistance-game.models';
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
    currentMissionRound: 0,
    missionTeam: [],
    missionSize: 0,
    gameStage: GameStage.GameStart,
    nextGameStage: GameStage.GameStart,
    gameOptions: {timeLimitMinutes: 0, botCount: 0},
    gameAction: GameAction.None,
    voteFailedCount: 0,
    missionRounds: new Map<number, boolean>(),
    missionOutcome: []
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
