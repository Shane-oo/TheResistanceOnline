import {Component, ElementRef, EventEmitter, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'app-game-stream-video',
  templateUrl: './game-stream-video.component.html',
  styleUrls: ['./game-stream-video.component.css']
})
export class GameStreamVideoComponent {
  @Output() makeCallEvent = new EventEmitter<ElementRef>();
  @ViewChild('remoteVideo') remoteVideo!: ElementRef;

  public makeCall() {
    if (this.remoteVideo) {
      this.makeCallEvent.emit();
    }
  }
}
