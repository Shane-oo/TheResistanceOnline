import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameValidatorService } from '../../shared/custom-validators/the-resistance-game/game-validator.service';
import { CreateGameCommand, GameDetails } from '../the-resistance-game.models';
import { AuthenticationService } from '../../user/authentication.service';

@Component({
             selector: 'app-create-game',
             templateUrl: './create-game.component.html',
             styleUrls: ['./create-game.component.css']
           })
export class CreateGameComponent implements OnInit {

  @Input() gameDetails: GameDetails = {
    userInGame: false,
    lobbyName: '',
    playersDetails: []
  };
  @Output() userJoinedLobby = new EventEmitter<string>();

  public createGameForm: FormGroup = new FormGroup({});
  // Angular checkboxes broken
  public chatChannel: boolean = true;
  public voiceChannel: boolean = false;
  @ViewChild('chatChannel') chatChannelElement!: ElementRef;
  @ViewChild('voiceChannel') voiceChannelElement!: ElementRef;

  constructor(private theResistanceGameService: TheResistanceGameService, private gameValidatorService: GameValidatorService, private authService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.createGameForm = new FormGroup({
                                          lobbyName: new FormControl('', [Validators.required]),
                                          chatChannel: new FormControl(true),
                                          voiceChannel: new FormControl(false)

                                        });
    // todo dispose of this when game starts?
    this.theResistanceGameService.addUserCreatedGameListener();

  }

  public validateControl = (controlName: string) => {
    return this.createGameForm.get(controlName)?.invalid && this.createGameForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.createGameForm.get(controlName)?.hasError(errorName);

  };

  public onlyOneValue(event: any) {
    if(event.target.id == 'chatChannel') {
      this.chatChannel = true;
      this.voiceChannel = false;
      this.voiceChannelElement.nativeElement.checked = false;
      this.chatChannelElement.nativeElement.checked = true;

    } else if(event.target.id == 'voiceChannel') {
      this.chatChannel = false;
      this.voiceChannel = true;
      this.voiceChannelElement.nativeElement.checked = true;
      this.chatChannelElement.nativeElement.checked = false;

    }
  }

  public createGame = (createGameFormValue: CreateGameCommand) => {
    const formValues = {...createGameFormValue};
    const createGameCommand: CreateGameCommand = {
      lobbyName: formValues.lobbyName,
      createChatChannel: this.chatChannel,
      createVoiceChannel: this.voiceChannel,
      userId: this.authService.getUserId()
    };
    // invoke createGame()
    this.theResistanceGameService.createGame(createGameCommand)


  };

}
