import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { JoinGameCommand } from '../the-resistance-game.models';

@Component({
             selector: 'app-join-game',
             templateUrl: './join-game.component.html',
             styleUrls: ['./join-game.component.css']
           })
export class JoinGameComponent implements OnInit {

  public joinGameForm: FormGroup = new FormGroup({});

  constructor(private gameService: TheResistanceGameService) {
  }

  ngOnInit(): void {
    this.joinGameForm = new FormGroup({
                                        lobbyName: new FormControl('poop', [Validators.required])
                                      });

    //todo add listeners here
    this.gameService.addUserJoinedGameListener();
    this.gameService.addGameIsFullListener();
    this.gameService.addGameDoesNotExistListener();
  }


  public validateControl = (controlName: string) => {
    return this.joinGameForm.get(controlName)?.invalid && this.joinGameForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.joinGameForm.get(controlName)?.hasError(errorName);
  };

  public joinGame = (joinGameFormValue: JoinGameCommand) => {
    const formValues = {...joinGameFormValue};
    const joinGameCommand: JoinGameCommand = {
      lobbyName: formValues.lobbyName
    };

    this.gameService.joinGame(joinGameCommand);
  };
}
