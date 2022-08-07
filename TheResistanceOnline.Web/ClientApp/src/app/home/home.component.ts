import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';

@Component({
             selector: 'app-home',
             templateUrl: './home.component.html'
           })
export class HomeComponent implements OnInit {


  public videoPlayer1!: HTMLVideoElement;

  constructor() {

  }

  @ViewChild('Video')
  set mainVideoEl1(el: ElementRef) {
    this.videoPlayer1 = el.nativeElement;
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {

    this.videoPlayer1.muted = true;
    this.videoPlayer1.play();


  }

  ngOnInit(): void {

  }


}
