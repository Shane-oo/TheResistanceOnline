import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../../user/authentication.service';

@Injectable({
              providedIn: 'root'
            })
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthenticationService, private router: Router) {
  }
  
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.authService.isUserAdmin();
  }
}
