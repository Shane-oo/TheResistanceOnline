import { Component, OnInit } from '@angular/core';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameAction, GameDetails, GameStage } from '../the-resistance-game.models';
import { ActivatedRoute, Router } from '@angular/router';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';


@Component({
             selector: 'app-join-game',
             templateUrl: './join-game.component.html',
             styleUrls: ['./join-game.component.css']
           })
export class JoinGameComponent implements OnInit {


  public groupNameToGameDetailsMap: Map<string, GameDetails> = new Map<string, GameDetails>();

  public selectedGameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isAvailable: false,
    currentMissionRound: 0,
    missionTeam: [],
    missionSize: 0,
    gameStage: GameStage.GameStart,
    nextGameStage: GameStage.GameStart,
    gameOptions: {timeLimitMinutes: 0, botCount: 0},
    gameAction: GameAction.None,
    voteFailedCount: 0,
    missionRounds: new Map<number, boolean>(),
    missionOutcome: []

  };


  constructor(private gameService: TheResistanceGameService, private swalService: SwalContainerService, private route: ActivatedRoute, private router: Router) {
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
  }

  ngOnDestroy() {
    // remove all non required listeners
    this.gameService.removeReceiveAllGameDetailsToPlayersNotInGameListener();
  }

  // orders the groupNameToGameDetailsMap in descending order
  asIsOrder(a: any, b: any) {
    return 1;
  }

  public changeGameDetails(gameDetails: GameDetails) {
    this.selectedGameDetails = gameDetails;
  }
}
