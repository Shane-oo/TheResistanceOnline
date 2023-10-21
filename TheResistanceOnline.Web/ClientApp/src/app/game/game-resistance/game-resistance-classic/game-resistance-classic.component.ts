import {AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild} from '@angular/core';
import {Subject, first, takeUntil} from "rxjs";
import {ResistanceGame} from "../resistance-game/resistance-game";
import {CommenceGameModel, Phase} from "../game-resistance.models";
import {ReadyUpCommand} from "../../game-lobby/game-lobby.models";


@Component({
  selector: 'app-game-resistance-classic',
  templateUrl: './game-resistance-classic.component.html',
  styleUrls: ['./game-resistance-classic.component.css']
})
export class GameResistanceClassicComponent implements AfterViewInit, OnDestroy {
  @Input() gameCommenced!: Subject<CommenceGameModel>;
  @Output() objectClickedEventEmitter = new EventEmitter<string>();

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

}
