import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../user/authentication.service';
import { Router } from '@angular/router';
import { SwalContainerService } from '../../ui/swal/swal-container.service';
import { SessionStorageService } from '../shared/services/session-storage.service';
import { UserDetailsModel } from '../user/user-edit/user-edit.models';

@Component({
             selector: 'app-nav-menu',
             templateUrl: './nav-menu.component.html',
             styleUrls: ['./nav-menu.component.css']
           })
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isUserAuthenticated = false;
  isUserAdmin = false;
  public _userDetails: UserDetailsModel | undefined;

  constructor(private authService: AuthenticationService, private sessionService: SessionStorageService, private router: Router, private swalService: SwalContainerService) {
    this.authService.authChanged.subscribe(res => {
      this.isUserAuthenticated = res;
      this.isUserAdmin = this.authService.isUserAdmin();
    });

  }

  ngOnInit(): void {
    this.authService.authChanged.subscribe(res => {
      this.isUserAuthenticated = res;
      this.isUserAdmin = this.authService.isUserAdmin();
    });
    this._userDetails = this.sessionService.getUserDetails();
  }


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.authService.logout();
    this.sessionService.removeSessionStorage();
    this.router.navigate(['/user']).then(r => {
    });
  }
}
