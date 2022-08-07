import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './user/authentication.service';

@Component({
             selector: 'app-root',
             templateUrl: './app.component.html'
           })
export class AppComponent implements OnInit {
  title = 'app';
  constructor(private authService :AuthenticationService) {
  }

  ngOnInit():void{
    console.log("in here")
           if(this.authService.isUserAuthenticated()){
             console.log(this.authService.isUserAuthenticated())
             this.authService.sendAuthStateChange(true)
           }
  }
}
