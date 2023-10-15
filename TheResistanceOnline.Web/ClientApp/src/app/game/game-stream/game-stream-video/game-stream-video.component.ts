import {AfterViewInit, Component, ElementRef, EventEmitter, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'app-game-stream-video',
  templateUrl: './game-stream-video.component.html',
  styleUrls: ['./game-stream-video.component.css']
})
export class GameStreamVideoComponent implements AfterViewInit {
  public remoteVideoRefs: ElementRef[] = [];
  @ViewChild('remoteVideo1') remoteVideo1!: ElementRef;
  @ViewChild('remoteVideo2') remoteVideo2!: ElementRef;
  @ViewChild('remoteVideo3') remoteVideo3!: ElementRef;
  @ViewChild('remoteVideo4') remoteVideo4!: ElementRef;
  @ViewChild('remoteVideo5') remoteVideo5!: ElementRef;
  @ViewChild('remoteVideo6') remoteVideo6!: ElementRef;
  @ViewChild('remoteVideo7') remoteVideo7!: ElementRef;
  @ViewChild('remoteVideo8') remoteVideo8!: ElementRef;
  @ViewChild('remoteVideo9') remoteVideo9!: ElementRef;


  ngAfterViewInit(): void {
    this.remoteVideoRefs = [
      this.remoteVideo1,
      this.remoteVideo2,
      this.remoteVideo3,
      this.remoteVideo4,
      this.remoteVideo5,
      this.remoteVideo6,
      this.remoteVideo7,
      this.remoteVideo8,
      this.remoteVideo9
    ];
  }
}
