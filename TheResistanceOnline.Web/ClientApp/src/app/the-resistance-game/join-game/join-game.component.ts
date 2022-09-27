import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { JoinGameCommand } from '../the-resistance-game.models';
import { ActivatedRoute, Router } from '@angular/router';
import { DiscordServerService } from '../../shared/services/discord-server.service';
import { CreateDiscordUserCommand } from '../../shared/models/discord-server.models';
import { HttpErrorResponse } from '@angular/common/http';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';

@Component({
             selector: 'app-join-game',
             templateUrl: './join-game.component.html',
             styleUrls: ['./join-game.component.css']
           })
export class JoinGameComponent implements OnInit {

  public joinGameForm: FormGroup = new FormGroup({});

  constructor(private gameService: TheResistanceGameService, private discordServerService: DiscordServerService, private swalService: SwalContainerService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit(): void {
    this.joinGameForm = new FormGroup({
                                        lobbyName: new FormControl('poop', [Validators.required])
                                      });
    // check to see if route contains discord access token
    let params = this.route.snapshot.fragment;
    if(params) {
      const data = JSON.parse(
        '{"' +
        decodeURI(params)
          .replace(/"/g, '\\"')
          .replace(/&/g, '","')
          .replace(/=/g, '":"') +
        '"}'
      );

      let createDiscordUserCommand: CreateDiscordUserCommand = {tokenType: data?.token_type, accessToken: data?.access_token};
      this.discordServerService.createDiscordUser(createDiscordUserCommand).subscribe({
                                                                                        next: (response: any) => {
                                                                                          this.swalService.showSwal(
                                                                                            'Discord Account Successfully Activated!',
                                                                                            SwalTypesModel.Success);
                                                                                          //ToDo show server invite if user is not in discord server
                                                                                        },
                                                                                        error: (err: HttpErrorResponse) => {
                                                                                          console.log(err);
                                                                                        }
                                                                                      });
    } else {
      this.gameService.addDiscordNotFoundListener();
    }

    //todo add listeners here
    this.gameService.addUserJoinedGameListener();
    this.gameService.addGameIsFullListener();
    this.gameService.addGameDoesNotExistListener();


  }

  ngOnDestroy() {
    console.log('join-game destroyed');
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
