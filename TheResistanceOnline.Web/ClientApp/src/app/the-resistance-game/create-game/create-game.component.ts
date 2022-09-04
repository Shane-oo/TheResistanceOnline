import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { CreateGameCommand } from '../the-resistance-game.models';

@Component({
             selector: 'app-create-game',
             templateUrl: './create-game.component.html',
             styleUrls: ['./create-game.component.css']
           })
export class CreateGameComponent implements OnInit {

  public createGameForm: FormGroup = new FormGroup({});
  // Angular checkboxes broken
  public chatChannel: boolean = true;
  public voiceChannel: boolean = false;
  @ViewChild('chatChannel') chatChannelElement!: ElementRef;
  @ViewChild('voiceChannel') voiceChannelElement!: ElementRef;

  constructor(private gameService: TheResistanceGameService) {
  }

  ngOnInit(): void {
    this.createGameForm = new FormGroup({
                                          lobbyName: new FormControl('', [Validators.required]),
                                          chatChannel: new FormControl(true),
                                          voiceChannel: new FormControl(false)

                                        });
    // todo dispose of this when game starts?
    this.gameService.addUserCreatedGameListener();
    this.gameService.addTooManyGamesListener();
    this.gameService.addGameAlreadyExistsListener();

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
      createVoiceChannel: this.voiceChannel
    };
    // invoke createGame()
    this.gameService.createGame(createGameCommand);
  };

}
