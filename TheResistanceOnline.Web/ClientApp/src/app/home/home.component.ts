import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';

@Component({
             selector: 'app-home',
             templateUrl: './home.component.html'
           })
export class HomeComponent implements OnInit {
  public showBackGround1 = true;
  public showBackGround2 = false;

  public videoPlayer1!: HTMLVideoElement;
  public videoPlayer2!: HTMLVideoElement;

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    if(this.showBackGround1) {
      this.videoPlayer1.muted = true;
      this.videoPlayer1.play();
    }
    if(this.showBackGround2) {
      this.videoPlayer2.muted = true;
      this.videoPlayer2.play();
    }
  }

  @ViewChild('Video')
  set mainVideoEl1(el: ElementRef) {
    if(this.showBackGround1) {
      this.videoPlayer1 = el.nativeElement;
    }
  }

  @ViewChild('Video2')
  set mainVideoEl2(el: ElementRef) {
    if(this.showBackGround2) {
      this.videoPlayer2 = el.nativeElement;
    }
  }

  constructor() {

  }

  async ngOnInit(): Promise<void> {
    await this.delay(18760);
    this.showBackGround2 = true;

    await this.delay(5000);
    this.showBackGround1 = false;
  }

  public delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

}
