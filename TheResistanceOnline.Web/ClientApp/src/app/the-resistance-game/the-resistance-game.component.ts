import { Component, OnInit } from '@angular/core';
import { GameAction, GameDetails, GameStage } from './the-resistance-game.models';
import { TheResistanceGameService } from './the-resistance-game.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
             selector: 'app-the-resistance-game',
             templateUrl: './the-resistance-game.component.html',
             styleUrls: ['./the-resistance-game.component.css']
           })
export class TheResistanceGameComponent implements OnInit {
  public userIsInGame: boolean = false;
  public playerId: string = '';
  public gameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false,
    missionRound: 0,
    missionTeam: [],
    missionSize: 0,
    gameStage: GameStage.GameStart,
    gameOptions: {timeLimitMinutes: 0, moveTimeLimitMinutes: 0, botCount: 0},
    gameAction: GameAction.None
  };
  public isTheHost: boolean = false;
  private readonly destroyed = new Subject<void>();

  constructor(private gameService: TheResistanceGameService) {

    this.gameService.gameDetailsChanged
        .pipe(takeUntil(this.destroyed))
        .subscribe((value: GameDetails) => {
          this.gameDetails = value;
          this.userIsInGame = true;
        });

    this.gameService.hostChanged
        .pipe(takeUntil(this.destroyed))
        .subscribe((value: boolean) => {
          this.isTheHost = value;
        });

    this.gameService.playerIdChanged
        .pipe(takeUntil(this.destroyed))
        .subscribe((value: string) => {
          this.playerId = value;
        });

    this.gameService.addReceiveGameDetailsListener();
    this.gameService.addReceiveGameHostListener();
    this.gameService.addReceivePlayerId();
  }

  async ngOnInit() {
    // start hub connection
    await this.gameService.start().then(r => console.log('connected'));
  }

  async ngOnDestroy() {
    // stop hub connection
    await this.gameService.stop();
    this.destroyed.next();
    this.destroyed.complete();
  }

}
