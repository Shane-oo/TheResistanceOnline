import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../user/authentication.service';
import {Router} from '@angular/router';
import {navbarDataLoggedIn, navbarDataNotLoggedIn} from './nav-data';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {
    isUserAuthenticated = false;
    isUserAdmin = false;

    navDataLoggedIn = navbarDataLoggedIn;
    navDataNotLoggedIn = navbarDataNotLoggedIn;

    constructor(private authService: AuthenticationService, private router: Router) {
        this.authService.authChanged.subscribe(res => {
            this.isUserAuthenticated = res;
            this.isUserAdmin = this.authService.isUserAdmin();
        });

    }

    ngOnInit(): void {
    }


    logout() {
        this.authService.logout();
        this.router.navigate(['/user/login']).then(r => {
        });
    }
}
