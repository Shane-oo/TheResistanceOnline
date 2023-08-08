import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {AuthenticationService} from "../../user/authentication.service";

export const adminGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthenticationService);
    const router = inject(Router);
    if (authService.isUserAdmin()) {
        return true;
    }
    // If they are not admin return them home
    router.navigate(['/'])
    return false;
};
