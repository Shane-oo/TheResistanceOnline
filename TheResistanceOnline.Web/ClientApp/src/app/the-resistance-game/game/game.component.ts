import { Component, Input, OnInit } from '@angular/core';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameDetails } from '../the-resistance-game.models';

@Component({
             selector: 'app-game',
             templateUrl: './game.component.html',
             styleUrls: ['./game.component.css']
           })
export class GameComponent implements OnInit {
  @Input() gameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false
  };
  @Input() playerId: string = '';
  public missionLeaderPlayerId: string = '';
  public middle: number = 0;

  constructor(private gameService: TheResistanceGameService) {
    this.middle = Math.ceil(this.gameDetails.playersDetails.length / 2);
  }

  ngOnInit(): void {
    this.gameService.addReceiveGameFinishedListener();
  }

  ngOnChanges(): void {
    this.missionLeaderPlayerId = this.gameDetails.playersDetails.find(p => p.isMissionLeader)!.playerId;
  }
}
