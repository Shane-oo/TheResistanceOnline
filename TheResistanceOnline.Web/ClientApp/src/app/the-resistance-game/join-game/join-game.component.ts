import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameDetails, JoinGameCommand } from '../the-resistance-game.models';
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
  public groupNameToGameDetailsMap: Map<string, GameDetails> = new Map<string, GameDetails>();

  public selectedGameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false
  };


  constructor(private gameService: TheResistanceGameService, private discordServerService: DiscordServerService, private swalService: SwalContainerService, private route: ActivatedRoute, private router: Router) {
    this.gameService.groupNameToGameDetailsMapChanged.subscribe((value: Map<string, GameDetails>) => {
      this.groupNameToGameDetailsMap = value;
      console.log('in subscription');
      console.log(this.groupNameToGameDetailsMap);
    });
  }


  ngOnInit(): void {
    // JoinGame Listeners
    this.gameService.addReceiveAllGameDetailsToPlayersNotInGameListener();


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
                                                                                          this.router.navigate([`/the-resistance-game`]).then(
                                                                                            r => {
                                                                                              this.swalService.showSwal(
                                                                                                'Discord Account Added!',
                                                                                                SwalTypesModel.Success);
                                                                                            });
                                                                                        },
                                                                                        error: (err: HttpErrorResponse) => {
                                                                                          console.log(err);
                                                                                        }
                                                                                      });
    } else {
      this.gameService.addDiscordNotFoundListener();
    }
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

  // orders the groupNameToGameDetailsMap in descending order
  asIsOrder(a: any, b: any) {
    return 1;
  }

  public changeGameDetails(gameDetails: GameDetails) {
    this.selectedGameDetails = gameDetails;
  }
}
