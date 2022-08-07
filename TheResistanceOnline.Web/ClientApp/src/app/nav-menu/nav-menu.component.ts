import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../user/authentication.service';
import { Router } from '@angular/router';
import { SwalContainerService, SwalTypesModel } from '../../ui/swal/swal-container.service';

@Component({
             selector: 'app-nav-menu',
             templateUrl: './nav-menu.component.html',
             styleUrls: ['./nav-menu.component.css']
           })
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isUserAuthenticated = false;

  constructor(private authService: AuthenticationService,private router:Router,private swalService:SwalContainerService) {
  }

  ngOnInit(): void {
    this.authService.authChanged.subscribe(res => {
      this.isUserAuthenticated = res;
    });
  }


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['/user/login']).then(r => {
      this.swalService.showSwal(
        'Successfully logged out',
        SwalTypesModel.Success);
    });
  }



}
