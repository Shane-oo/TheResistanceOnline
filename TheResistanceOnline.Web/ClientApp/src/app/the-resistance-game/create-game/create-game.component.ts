import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameValidatorService } from '../../shared/custom-validators/the-resistance-game/game-validator.service';
import { CreateGameCommand } from '../the-resistance-game.models';
import { HttpErrorResponse } from '@angular/common/http';

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

  constructor(private theResistanceGameService: TheResistanceGameService, private gameValidatorService: GameValidatorService) {
  }

  ngOnInit(): void {
    this.createGameForm = new FormGroup({
                                          lobbyName: new FormControl('', [Validators.required]),
                                          chatChannel: new FormControl(true),
                                          voiceChannel: new FormControl(false)

                                        });

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

  // todo create interface
  public createGame = (createGameFormValue: CreateGameCommand) => {
    const formValues = {...createGameFormValue};
    const createGameCommand: CreateGameCommand = {
      lobbyName: formValues.lobbyName,
      createChatChannel: this.chatChannel,
      createVoiceChannel: this.voiceChannel
    };

    this.theResistanceGameService.createGame(createGameCommand).subscribe({
                                                                            next: () => {
                                                                              // this.router.navigate([`/user/login`]).then(r => {
                                                                              //   this.swalService.showSwal(
                                                                              //     'Successfully registered! An email has been sent to '
                                                                              //     + user.email
                                                                              //     + ' please follow the link provided to confirm email address.',
                                                                              //     SwalTypesModel.Success);
                                                                              // });
                                                                              // todo navigate to pre game lobby
                                                                            },
                                                                            error: (err: HttpErrorResponse) => {
                                                                            }
                                                                          });
  };

}
