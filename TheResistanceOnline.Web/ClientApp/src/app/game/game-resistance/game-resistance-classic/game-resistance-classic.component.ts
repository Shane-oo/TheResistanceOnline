import {AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild} from '@angular/core';
import {first, Subject, takeUntil} from "rxjs";

import {ResistanceGame} from "../resistance-game/resistance-game";
import {CommenceGameModel, Phase} from "../game-resistance.models";


@Component({
  selector: 'app-game-resistance-classic',
  templateUrl: './game-resistance-classic.component.html',
  styleUrls: ['./game-resistance-classic.component.css']
})
export class GameResistanceClassicComponent implements AfterViewInit, OnDestroy {
  @Input() gameCommenced!: Subject<CommenceGameModel>;
  @Input() newMissionTeamMember!: Subject<string>;
  @Input() removeMissionTeamMember!: Subject<string>;
  @Input() moveToVotingPhase!: Subject<string[]>;
  @Input() moveToVoteResultsPhase!: Subject<void>;
  @Input() playerSubmittedVote!: Subject<string>;

  @Input() showMissionTeamSubmit: boolean = false;

  @Output() objectClickedEventEmitter = new EventEmitter<string>();
  @Output() submitMissionTeamEventEmitter = new EventEmitter<void>();

  private resistanceGame!: ResistanceGame;
  private readonly destroyed = new Subject<void>();

  // Get canvas
  @ViewChild('canvas')
  private canvasElementRef!: ElementRef;

  constructor() {
  }

  ngAfterViewInit(): void {
    this.resistanceGame = new ResistanceGame(this.canvasElementRef.nativeElement);

    // Resistance Game Events
    this.resistanceGame.objectClickedSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe((name: string) => {
        this.objectClickedEventEmitter.emit(name);
      })

    this.gameCommenced
      .pipe(first()) //This will automatically complete (and therefore unsubscribe) after the first value has been emitted.
      .subscribe(c => {
        this.commenceGame(c);
      });

    this.newMissionTeamMember
      .pipe(takeUntil(this.destroyed))
      .subscribe(((selectedPlayerName: string) => {
        this.addMissionTeamMember(selectedPlayerName);
      }));

    this.removeMissionTeamMember
      .pipe(takeUntil(this.destroyed))
      .subscribe(((selectedPlayerName: string) => {
        this.deleteMissionTeamMember(selectedPlayerName);
      }));

    this.moveToVotingPhase
      .pipe(takeUntil(this.destroyed))
      .subscribe((missionTeamMembers: string[]) => {
        this.startVotePhase(missionTeamMembers);
      });

    this.moveToVoteResultsPhase
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        this.startVoteResultsPhase();
      });


    this.playerSubmittedVote
      .pipe(takeUntil(this.destroyed))
      .subscribe((playerName: string) => {
        this.playerVoted(playerName);
      });


  }

  ngOnDestroy(): void {
    this.destroyed.next();
    this.destroyed.complete();
    this.resistanceGame.destroy();
  }

  commenceGame(gameCommenced: CommenceGameModel) {
    this.resistanceGame.setPlayers(gameCommenced.players);
    this.resistanceGame.setMissionLeader(gameCommenced.missionLeader);

    switch (gameCommenced.phase) {
      case Phase.MissionBuild:
        if (gameCommenced.isMissionLeader) {
          let missionMembers = 0;
          switch (gameCommenced.players.length) {
            case 5:
            case 6:
            case 7:
              missionMembers = 2;
              break;
            case 8:
            case 9:
            case 10:
              missionMembers = 3;
              break;
          }
          this.resistanceGame.setMissionBuildPhase(missionMembers);
        }

        break;
    }
  }

  submitMissionTeam() {
    this.submitMissionTeamEventEmitter.emit();
  }

  private addMissionTeamMember(playerName: string) {
    this.resistanceGame.addMissionTeamMember(playerName);
  }

  private deleteMissionTeamMember(playerName: string) {
    this.resistanceGame.removeMissionTeamMember(playerName);
  }

  private startVotePhase(missionTeamMembers: string[]) {
    this.resistanceGame.startVotePhase(missionTeamMembers);
  }

  private startVoteResultsPhase() {
    this.resistanceGame.startVoteResultsPhase();
  }

  private playerVoted(playerName: string) {
    this.resistanceGame.playerVoted(playerName);
  }

}
