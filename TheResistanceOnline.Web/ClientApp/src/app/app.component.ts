import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from './user/authentication.service';
import {SideNavToggle} from './nav-menu/nav-data';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
    title = 'app';
    isSideNavCollapsed = false;
    screenWidth = 0;

    constructor(private authService: AuthenticationService) {
    }

    ngOnInit(): void {
        if (this.authService.isUserAuthenticated()) {
            this.authService.sendAuthStateChange(true);
        }
    }

}
