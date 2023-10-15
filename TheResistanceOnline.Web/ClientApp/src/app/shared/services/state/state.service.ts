import {Injectable} from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class StateService {
    private readonly returnUrlLocalStorageKey = "TheResistanceOnline.Web.RedirectUrl"
    private readonly userNameLocalStorageKey = "TheResistanceOnline.Web.UserName";

    constructor() {
    }

    get returnUrl() {
        return localStorage.getItem(this.returnUrlLocalStorageKey) ?? "/";
    }

    set returnUrl(value: string) {
        localStorage.setItem(this.returnUrlLocalStorageKey, value);
    }

    get userName() {
        return localStorage.getItem(this.userNameLocalStorageKey) ?? "";
    }

    set userName(value: string) {
        localStorage.setItem(this.userNameLocalStorageKey, value);
    }

}
