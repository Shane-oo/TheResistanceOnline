import {AfterViewInit, Component, ElementRef, OnDestroy, ViewChild} from '@angular/core';

import {ResistanceGame} from "./resistance-game/resistance-game";


@Component({
  selector: 'app-resistance-visualisation',
  templateUrl: './resistance-visualisation.component.html',
  styleUrls: ['./resistance-visualisation.component.css']
})
export class ResistanceVisualisationComponent implements AfterViewInit, OnDestroy {
  private resistanceGame!: ResistanceGame;

  // Get canvas
  @ViewChild('canvas')
  private canvasElementRef!: ElementRef;

  constructor() {
  }

  ngAfterViewInit(): void {
    this.resistanceGame = new ResistanceGame(this.canvasElementRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.resistanceGame.destroy();
  }


}
