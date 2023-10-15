import {AfterViewInit, Component, ElementRef, Input, OnDestroy, ViewChild} from '@angular/core';
import {Subject, first, takeUntil} from "rxjs";
import {ResistanceGame} from "../resistance-game/resistance-game";
import {CommenceGameModel} from "../game-resistance.models";


@Component({
  selector: 'app-game-resistance-classic',
  templateUrl: './game-resistance-classic.component.html',
  styleUrls: ['./game-resistance-classic.component.css']
})
export class GameResistanceClassicComponent implements AfterViewInit, OnDestroy {
  @Input() gameCommenced!: Subject<CommenceGameModel>;
  private resistanceGame!: ResistanceGame;
  private readonly destroyed = new Subject<void>();

  // Get canvas
  @ViewChild('canvas')
  private canvasElementRef!: ElementRef;

  constructor() {
  }

  ngAfterViewInit(): void {
    this.resistanceGame = new ResistanceGame(this.canvasElementRef.nativeElement);

    this.gameCommenced
      .pipe(first()) //This will automatically complete (and therefore unsubscribe) after the first value has been emitted.
      .subscribe(c => {
        this.commenceGame(c);
      });
  }

  ngOnDestroy(): void {
    this.destroyed.next();
    this.resistanceGame.destroy();
  }

  commenceGame(gameCommenced: CommenceGameModel) {
    console.log("MY GAME COMMENCED", gameCommenced)
    this.resistanceGame.setPlayers(gameCommenced.players);
  }

}
