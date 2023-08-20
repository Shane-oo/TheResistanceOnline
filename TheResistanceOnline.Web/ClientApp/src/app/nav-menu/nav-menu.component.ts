import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {navbarDataLoggedIn, navbarDataNotLoggedIn} from './nav-data';

import {AuthenticationService} from "../shared/services/authentication/authentication.service";
import {StateService} from "../shared/services/state/state.service";

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

  constructor(private authService: AuthenticationService,
              private stateService: StateService,
              private _router: Router) {
    this.authService.authChangedObservable.subscribe((res: boolean) => {
      this.isUserAuthenticated = res;
      this.isUserAdmin = this.authService.isUserAdmin();
    });

  }

  ngOnInit(): void {
  }


  logout() {
    this.authService.logOut();
    this._router.navigate(['/']);
  }

  get userName(): string {
    return this.stateService.userName;
  }
}
