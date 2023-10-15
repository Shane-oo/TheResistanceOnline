import {Injectable} from '@angular/core';
import {Observable, Subject} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AuthenticationModel, Roles} from "../../models/authentication.models";
import {environment} from "../../../../environments/environment";
import {StateService} from "../state/state.service";


@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {

    private readonly authenticationLocalStorageKey = "TheResistanceOnline.Web.Authentication"
    private readonly _clientId = "TheResistanceOnline.Web"
    private authChangedSubject = new Subject<boolean>();

    public authChangedObservable = this.authChangedSubject.asObservable();

    constructor(private readonly stateService: StateService, private readonly http: HttpClient) {
    }

    get clientId() {
        return this._clientId;
    }

    get authenticationData(): AuthenticationModel | null {
        let storedAuthentication = localStorage.getItem(this.authenticationLocalStorageKey);
        if (storedAuthentication) {
            return JSON.parse(storedAuthentication) as AuthenticationModel;
        }
        this.sendAuthStateChangeNotification(false);
        return null;
    }

    set authenticationData(data: AuthenticationModel) {
        localStorage.setItem(this.authenticationLocalStorageKey, JSON.stringify(data));
    }


    authorize(redirectUri: string, provider: string): void {
        const clientId = this.clientId;

        // hack to get around CORS
        window.location.href = `${environment.API_URL}` +
            `/Authorize` +
            `?response_type=code` +
            `&client_id=${clientId}` +
            `&redirect_uri=${redirectUri}` +
            `&provider=${provider}`;
    }

    logInWithAuthorizationCode(code: string, redirectUri: string): Observable<AuthenticationModel> {
        const data = `grant_type=authorization_code` +
            `&code=${code}` +
            `&client_id=${this.clientId}` +
            `&redirect_uri=${redirectUri}`;

        const headers = new HttpHeaders({'Content-Type': 'application/x-www-form-urlencoded'});
        return this.http.post<AuthenticationModel>(environment.API_URL + "/Token", data, {headers: headers});

    }

    refreshToken(): Observable<AuthenticationModel> {
        const authenticationData = this.authenticationData;
        const data = `grant_type=refresh_token` +
            `&refresh_token=${authenticationData?.refresh_token}` +
            `&client_id=${this.clientId}`
        const headers = new HttpHeaders({'Content-Type': 'application/x-www-form-urlencoded'});

        return this.http.post<AuthenticationModel>(environment.API_URL + "/Token", data, {headers: headers})
    }

    isUserAdmin(): boolean {
        if (this.isUserAuthenticated()) {
            return this.authenticationData?.role === Roles.Admin;
        }
        return false;
    }

    sendAuthStateChangeNotification(isAuthenticated: boolean) {
        this.authChangedSubject.next(isAuthenticated);
    }


    isUserAuthenticated(): boolean {
        // probably better ways to do this, maybe actually check token is still valid?
        return !!localStorage.getItem(this.authenticationLocalStorageKey);
    }

    logOut(): void {
        localStorage.removeItem(this.authenticationLocalStorageKey);
        this.stateService.userName = "";
        this.sendAuthStateChangeNotification(false);
    }
}
