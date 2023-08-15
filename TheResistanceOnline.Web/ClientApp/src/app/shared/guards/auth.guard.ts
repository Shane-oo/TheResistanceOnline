import {CanActivateFn, Router,} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../services/authentication/authentication.service";
import {StateService} from "../services/state/state.service";

export const authorizationGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthenticationService);
    const stateService = inject(StateService);
    const router = inject(Router);
    if (authService.isUserAuthenticated()) {
        return true;
    }
    stateService.returnUrl = state.url;
    router.navigate(['/user/login'])
    return false;
};
