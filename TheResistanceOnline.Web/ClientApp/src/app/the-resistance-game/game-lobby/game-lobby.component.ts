import { Component, Input, OnInit } from '@angular/core';
import { GameAction, GameDetails, GameOptions, GameStage, StartGameCommand } from '../the-resistance-game.models';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { faDiscord } from '@fortawesome/free-brands-svg-icons';

interface TimeLimit {
  minutesString: string;
  minutes: number;
}

@Component({
             selector: 'app-game-lobby',
             templateUrl: './game-lobby.component.html',
             styleUrls: ['./game-lobby.component.css']
           })
export class GameLobbyComponent implements OnInit {
  public discordIcon = faDiscord;
  @Input() gameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false,
    missionRound: 0,
    missionTeam: [],
    missionSize: 0,
    gameStage: GameStage.GameStart,
    nextGameStage: GameStage.GameStart,
    gameOptions: {timeLimitMinutes: 0, botCount: 0},
    gameAction: GameAction.None,
    voteFailedCount: 0
  };
  @Input() isTheHost: boolean = false;
  public gameOptionsForm: FormGroup = new FormGroup({});
  public middle: number = 0;
  private MAX_PLAYER_COUNT = 10;
  private MIN_PLAYER_COUNT = 5;
  private defaultTimeLimit: TimeLimit = {
    minutesString: '45 Minutes',
    minutes: 45
  };
  public timeLimitOptions: TimeLimit[] = [{minutesString: '30 Minutes', minutes: 30}, this.defaultTimeLimit,
    {minutesString: '60 Minutes', minutes: 60}, {minutesString: '90 Minutes', minutes: 90}];


  constructor(private gameService: TheResistanceGameService) {
  }

  ngOnInit(): void {
    this.middle = Math.ceil(this.gameDetails.playersDetails.length / 2);
    this.gameOptionsForm = new FormGroup({
                                           timeLimitMinutes: new FormControl(this.defaultTimeLimit.minutes, [Validators.required]),
                                           botCount: new FormControl(0,
                                                                     [
                                                                       Validators.max(this.MAX_PLAYER_COUNT
                                                                                      - this.gameDetails.playersDetails.length),
                                                                       Validators.min(0)
                                                                     ]),
                                           playerCount: new FormControl(this.gameDetails.playersDetails.length,
                                                                        [Validators.min(this.MIN_PLAYER_COUNT)])
                                         });
  }


  ngOnChanges() {
    this.middle = Math.ceil(this.gameDetails.playersDetails.length / 2);
    this.gameOptionsForm.get('playerCount')?.updateValueAndValidity();
    this.gameOptionsForm.get('botCount')?.updateValueAndValidity();
    this.botCountChanged();
  }

  ngOnDestroy() {
    // remove all non required listeners
    this.gameService.removeReceivePlayerId();
    this.gameService.removeReceiveGameHostListener();
  }

  public validateControl = (controlName: string) => {
    return this.gameOptionsForm.get(controlName)?.invalid && this.gameOptionsForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.gameOptionsForm.get(controlName)?.hasError(errorName);

  };

  public startGame = (gameOptionsFormValue: GameOptions) => {
    const formValues = {...gameOptionsFormValue};
    console.log(formValues);
    const startGameCommand: StartGameCommand = {
      gameOptions: {
        timeLimitMinutes: formValues.timeLimitMinutes,
        botCount: formValues.botCount
      }
    };
    this.gameService.startGame(startGameCommand);

  };

  public botCountChanged = () => {
    let botCount = this.gameOptionsForm.get('botCount')?.value;
    if(botCount < this.MAX_PLAYER_COUNT - this.gameDetails.playersDetails.length) {
      this.gameOptionsForm.get('playerCount')?.patchValue(botCount + this.gameDetails.playersDetails.length);
    }
  };

  public leaveGame = () => {
    location.reload();
  };

  // for example of submit -> start game
  // public registerUser = (registerFormValue: UserRegisterModel) => {
  //   const formValues = {...registerFormValue};
  //   const user: UserRegisterModel = {
  //     userName: formValues.userName,
  //     email: formValues.email,
  //     password: formValues.password,
  //     confirmPassword: formValues.confirmPassword,
  //     clientUri: `${environment.Base_URL}/user/email-confirmation`
  //   };
  // };
}
