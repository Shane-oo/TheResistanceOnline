import {CanActivateFn, Router,} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../../user/authentication.service";

export const authorizationGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthenticationService);
    const router = inject(Router);
    if (authService.isUserAuthenticated()) {
        return true;
    }
    //todo localStorage.setItem('ReturnUrl', state.url);
    router.navigate(['/user/login'])
    return false;
};
