import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{
  public showBackGround1 = true;
  public showBackGround2 = false;


  constructor() {
  }
  async ngOnInit(): Promise<void> {
    await this.delay(18760);
    this.showBackGround2 = true;

    await this.delay(5000);
    this.showBackGround1 = false;
  }

   public delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }

}
