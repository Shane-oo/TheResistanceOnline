import { Component, OnInit } from '@angular/core';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameDetails } from '../the-resistance-game.models';
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

  public showDiscordWidget: boolean = true;

  public groupNameToGameDetailsMap: Map<string, GameDetails> = new Map<string, GameDetails>();

  public selectedGameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false
  };


  constructor(private gameService: TheResistanceGameService, private discordServerService: DiscordServerService, private swalService: SwalContainerService, private route: ActivatedRoute, private router: Router) {
    this.gameService.groupNameToGameDetailsMapChanged.subscribe((value: Map<string, GameDetails>) => {
      const map = new Map(Object.entries(value));
      this.groupNameToGameDetailsMap = map;

      if(this.selectedGameDetails?.channelName.length > 0) {
        this.selectedGameDetails = map.get(this.selectedGameDetails.channelName);
      }
    });
  }


  ngOnInit(): void {
    // JoinGame Listeners
    this.gameService.addReceiveAllGameDetailsToPlayersNotInGameListener();
    this.gameService.addReceiveDiscordUserNotInDiscordServerListener();
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
    // remove all non required listeners
    this.gameService.removeReceiveAllGameDetailsToPlayersNotInGameListener();
    this.gameService.removeDiscordNotFoundListener();
    this.gameService.removeReceiveDiscordUserNotInDiscordServerListener();
  }

  // orders the groupNameToGameDetailsMap in descending order
  asIsOrder(a: any, b: any) {
    return 1;
  }

  public changeGameDetails(gameDetails: GameDetails) {
    this.selectedGameDetails = gameDetails;
    this.toggleDiscordWidget(true);
  }

  public toggleDiscordWidget(offPermanently:boolean) {
    if(offPermanently){
      this.showDiscordWidget = false;
    }
    else {
      this.showDiscordWidget = !this.showDiscordWidget;
    }
  }
}
