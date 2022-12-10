import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { TheResistanceGameService } from '../the-resistance-game.service';
import { GameDetails, GameStage, TeamModel } from '../the-resistance-game.models';

import { faPersonMilitaryRifle, faPersonRifle, faSquarePlus} from '@fortawesome/free-solid-svg-icons';
import {faDiscord} from '@fortawesome/free-brands-svg-icons';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';
import { CountdownComponent, CountdownConfig, CountdownEvent } from 'ngx-countdown';


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
  public missionLeaderPlayerId: string = '';

  @Input() gameDetails: GameDetails = {
    channelName: '',
    playersDetails: [],
    isVoiceChannel: false,
    isAvailable: false,
    missionRound: 0,
    missionTeam: [],
    missionSize: 0,
    gameStage: GameStage.GameStart,
    gameOptions: {timeLimitMinutes: 0, moveTimeLimitMinutes: 0, botCount: 0}
  };
  @Input() playerId: string = '';

  public gameCountdownConfig: CountdownConfig = {leftTime: 0};
  public moveCountdownConfig: CountdownConfig = {leftTime: 0, format: 'm:s'};
  @ViewChild('gameCountdown', {static: false}) private gameCountdown!: CountdownComponent;
  @ViewChild('moveCountdown', {static: false}) private moveCountdown!: CountdownComponent;

  constructor(private gameService: TheResistanceGameService, private swalService: SwalContainerService) {
  }

  ngOnInit(): void {
    this.gameService.addReceiveGameFinishedListener();
    this.gameCountdownConfig.leftTime = this.gameDetails.gameOptions.timeLimitMinutes * 60;
    this.moveCountdownConfig.leftTime = this.gameDetails.gameOptions.moveTimeLimitMinutes * 60;

    this.notifySpies();
    this.notifyResistance();
    this.gameDetails.gameStage = GameStage.MissionPropose;
  }

  ngOnChanges(): void {
    this.missionLeaderPlayerId = this.gameDetails.playersDetails.find(p => p.isMissionLeader)!.playerId;
    console.log(this.gameDetails);
  }

  ngOnDestroy():void{

  }

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

  submitTeam = () => {
    this.moveCountdown.restart();
    console.log('user wants this team', this.gameDetails.missionTeam);
  };

  //moveCountdown Expired player took too long to choose
  handleCountdownEvent = (e: CountdownEvent) => {

    if(e.action === 'done') {
      //  this.notify += ` - ${e.left} ms`;
      console.log('countdown done');
      // todo send to backend  or actually backend will send to here when actually done
    }
    // todo is it possible to warn them turn is almost up
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
}
