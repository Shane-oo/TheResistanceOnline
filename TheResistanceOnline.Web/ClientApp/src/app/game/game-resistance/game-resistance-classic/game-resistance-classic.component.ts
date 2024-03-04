import {AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild} from '@angular/core';
import {first, Subject, takeUntil} from "rxjs";

import {ResistanceGame} from "../resistance-game/resistance-game";
import {CommenceGameModel, VoteResultsModel} from "../game-resistance.models";


@Component({
  selector: 'app-game-resistance-classic',
  templateUrl: './game-resistance-classic.component.html',
  styleUrls: ['./game-resistance-classic.component.css']
})
export class GameResistanceClassicComponent implements AfterViewInit, OnDestroy {
  @Input() gameCommenced!: Subject<CommenceGameModel>;
  @Input() startMissionBuildPhase!: Subject<void>;
  @Input() newMissionTeamMember!: Subject<string>;
  @Input() removeMissionTeamMember!: Subject<string>;
  @Input() moveToVotingPhase!: Subject<string[]>;
  @Input() removeVotingChoicesSubject!: Subject<void>;
  @Input() playerSubmittedVote!: Subject<string>;
  @Input() showVoteResultsSubject!: Subject<VoteResultsModel>;
  @Input() showMissionCardsSubject!: Subject<boolean>;

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

    this.startMissionBuildPhase
      .pipe(takeUntil(this.destroyed))
      .subscribe(c => {
        this.setMissionBuildPhase();
      })

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

    this.removeVotingChoicesSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        this.removeVotingChoices();
      });


    this.playerSubmittedVote
      .pipe(takeUntil(this.destroyed))
      .subscribe((playerName: string) => {
        this.playerVoted(playerName);
      });


    this.showVoteResultsSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe((results: VoteResultsModel) => {
        this.showVoteResults(results);
      });

    this.showMissionCardsSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe((showSuccessAndFail: boolean)=>{
        this.showMissionCards(showSuccessAndFail);
      })

  }

  ngOnDestroy(): void {
    this.destroyed.next();
    this.destroyed.complete();
    this.resistanceGame.destroy();
  }

  //
  commenceGame(gameCommenced: CommenceGameModel) {
    this.resistanceGame.setPlayers(gameCommenced.players);
    this.resistanceGame.setMissionLeader(gameCommenced.missionLeader);
  }

  setMissionBuildPhase() {
    this.resistanceGame.setMissionBuildPhase();
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

  private removeVotingChoices() {
    this.resistanceGame.removeVotingChoices();
  }

  private playerVoted(playerName: string) {
    this.resistanceGame.playerVoted(playerName);
  }

  private showVoteResults(results: VoteResultsModel) {
    this.resistanceGame.showVoteResults(results);
    // After 9 seconds remove vote results can't be bothered putting this on the server
    setTimeout(() => {
      this.resistanceGame.removeVoteResults();
    }, 9000);
  }

  private showMissionCards(showSuccessAndFail: boolean){
    this.resistanceGame.showMissionCards(showSuccessAndFail);
  }

}
