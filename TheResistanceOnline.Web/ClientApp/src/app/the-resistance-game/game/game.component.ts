import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameAction, GameDetails, GameStage, PlayerDetails, TeamModel } from '../the-resistance-game.models';

import {
  faCircleArrowRight,
  faCircleCheck,
  faCircleXmark,
  faPersonMilitaryRifle,
  faPersonRifle,
  faSquarePlus,
  faXmark
} from '@fortawesome/free-solid-svg-icons';
import { faDiscord } from '@fortawesome/free-brands-svg-icons';
import { SwalContainerService } from '../../../ui/swal/swal-container.service';
import { CountdownComponent, CountdownConfig } from 'ngx-countdown';


@Component({
             selector: 'app-game',
             templateUrl: './game.component.html',
             styleUrls: ['./game.component.css']
           })
export class GameComponent implements OnInit {
  public missionTeamMemberIcon = faPersonRifle;
  public missionLeaderIcon = faPersonMilitaryRifle;
  public addMemberIcon = faSquarePlus;
  public discordIcon = faDiscord;
  public crossIcon = faCircleXmark;
  public tickIcon = faCircleCheck;
  public nextIcon = faCircleArrowRight;
  public xMarkIcon = faXmark;

  public missionLeaderPlayerId: string = '';

  public voted: boolean = false;
  public continued: boolean = false;
  public chose: boolean = false;

  @Input() gameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
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
  @Input() playerId: string = '';

  public gameCountdownConfig: CountdownConfig = {leftTime: 0};
  @ViewChild('gameCountdown', {static: false}) private gameCountdown!: CountdownComponent;

  constructor(private gameService: TheResistanceGameService, private swalService: SwalContainerService) {
  }

  ngOnInit(): void {
    this.gameService.addReceiveGameFinishedListener();
    this.gameCountdownConfig.leftTime = this.gameDetails.gameOptions.timeLimitMinutes * 60;

    this.notifySpies();
    this.notifyResistance();
    //this.gameDetails.gameStage = GameStage.MissionPropose;

  }

  ngOnChanges(): void {
    this.missionLeaderPlayerId = this.gameDetails.playersDetails.find(p => p.isMissionLeader)!.playerId;
    console.log(this.gameDetails);
    this.voted = this.getPlayerDetails().voted;
    this.continued = this.getPlayerDetails().continued;
    this.chose = this.getPlayerDetails().chose;
    console.log(this.gameDetails.voteFailedCount);

    if(this.gameDetails.gameStage === GameStage.GameOverSpiesWon) {
      console.log('spies won');
    }
  }

  ngOnDestroy(): void {

  }

  getPlayerDetails = (): PlayerDetails => {
    return this.gameDetails.playersDetails.find(p => p.playerId === this.playerId)!;
  };
  teamMemberClicked = (selectedPlayerId: string) => {
    let selectedPlayerDetails = this.gameDetails.playersDetails.find(p => p.playerId === selectedPlayerId);
    if(selectedPlayerDetails) {
      selectedPlayerDetails.selectedTeamMember = !selectedPlayerDetails.selectedTeamMember;
      if(selectedPlayerDetails.selectedTeamMember) {
        if(this.gameDetails.missionTeam.length < this.gameDetails.missionSize) {
          this.gameDetails.missionTeam.push(selectedPlayerDetails);
        } else {
          //this.swalService.showSwal('Mission Team Full', SwalTypesModel.Error);
          // not sure if should do anything here
        }
      } else {
        this.gameDetails.missionTeam = this.gameDetails.missionTeam.filter(p => p !== selectedPlayerDetails);
      }
    }

  };
  playerIsOnMissionTeam = (playerId: string) => {

    return this.gameDetails.missionTeam.some(p => p.playerId === playerId);
  };

  submitTeam = () => {
    this.gameDetails.gameAction = GameAction.SubmitMissionPropose;
    let gameActionCommand = {
      gameDetails: this.gameDetails
    };
    this.gameService.sendGameActionCommand(gameActionCommand);
  };

  submitVote = (approved: boolean) => {
    let playerDetails = this.getPlayerDetails();
    playerDetails.voted = true;
    playerDetails.approvedMissionTeam = approved;

    this.gameDetails.gameAction = GameAction.SubmitVote;
    this.sendGameActionCommand();
  };

  submitContinue = () => {
    let playerDetails = this.getPlayerDetails();
    playerDetails.continued = true;

    this.gameDetails.gameAction = GameAction.Continue;
    this.sendGameActionCommand();
  };

  submitMissionChoice = (supported: boolean) => {
    let playerDetails = this.getPlayerDetails();
    playerDetails.supportedMission = supported;
    playerDetails.chose = true;

    this.gameDetails.gameAction = GameAction.SubmitMissionChoice;
    this.sendGameActionCommand();
  };

  // GameStage.GameStart
  notifySpies = () => {
    const spies = this.gameDetails.playersDetails.filter(p => p.team === TeamModel.Spy);
    if(spies.some(p => p.playerId === this.playerId)) {
      this.swalService.fireNotifySpiesModal(spies);
    }
  };

  notifyResistance = () => {
    const resistance = this.gameDetails.playersDetails.filter(p => p.team === TeamModel.Resistance);
    if(resistance.some(p => p.playerId === this.playerId)) {
      this.swalService.fireNotifyResistanceModal();
    }
  };

  getMissionLeaderUsername = (): string => {
    const missionLeader = this.gameDetails.playersDetails!.find(p => p.playerId === this.missionLeaderPlayerId);
    if(missionLeader) {
      return missionLeader.userName!;
    }
    return '';
  };

  private sendGameActionCommand = () => {
    let gameActionCommand = {
      gameDetails: this.gameDetails
    };
    this.gameService.sendGameActionCommand(gameActionCommand);
  };


  /*  handleCountdownEvent = (e: CountdownEvent) => {

      if(e.action === 'done') {
        //  this.notify += ` - ${e.left} ms`;
        console.log('countdown done');
        // only host sends so that backend does not get bombarded
        this.submitContinue();
      }
    };*/
}
