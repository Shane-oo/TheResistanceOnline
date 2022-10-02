import { Component, Input, OnInit } from '@angular/core';
import { GameDetails } from '../../the-resistance-game.models';

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

  constructor() {
  }

  ngOnInit(): void {
  }

}
