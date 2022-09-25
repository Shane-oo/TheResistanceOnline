import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { AuthenticationService } from '../user/authentication.service';
import { Router } from '@angular/router';
import { navbarDataLoggedIn, navbarDataNotLoggedIn, SideNavToggle } from './nav-data';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
             selector: 'app-nav-menu',
             templateUrl: './nav-menu.component.html',
             styleUrls: ['./nav-menu.component.css'],
             animations: [
               trigger('fadeInOut', [
                 transition(':enter', [
                   style({opacity: 0}),
                   animate('350ms',
                           style({
                                   opacity: 1
                                 }))
                 ]),
                 transition(':leave', [
                   style({opacity: 1}),
                   animate('350ms',
                           style({
                                   opacity: 0
                                 }))
                 ])
               ])
             ]
           })
export class NavMenuComponent implements OnInit {
  isUserAuthenticated = false;
  isUserAdmin = false;

  @Output() onToggleSideNav: EventEmitter<SideNavToggle> = new EventEmitter<SideNavToggle>();
  collapsed = false;
  screenWidth = 0;
  navDataLoggedIn = navbarDataLoggedIn;
  navDataNotLoggedIn = navbarDataNotLoggedIn;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.authChanged.subscribe(res => {
      this.isUserAuthenticated = res;
      this.isUserAdmin = this.authService.isUserAdmin();
    });

  }

  ngOnInit(): void {
    this.screenWidth = window.innerWidth;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.screenWidth = window.innerWidth;
    if(this.screenWidth <= 768) {
      this.collapsed = false;
      this.onToggleSideNav.emit({collapsed: this.collapsed, screenWidth: this.screenWidth});
    }
  }


  logout() {
    this.authService.logout();
    this.router.navigate(['/user/login']).then(r => {
    });
  }

  toggleCollapse(): void {
    this.collapsed = !this.collapsed;
    this.onToggleSideNav.emit({collapsed: this.collapsed, screenWidth: this.screenWidth});

  }

  closeSidenav(): void {
    this.collapsed = false;
    this.onToggleSideNav.emit({collapsed: this.collapsed, screenWidth: this.screenWidth});
  }
}
