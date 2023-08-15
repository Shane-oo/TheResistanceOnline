import {Component, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HttpErrorResponse} from '@angular/common/http';
import {DOCUMENT} from "@angular/common";
import {Subject, takeUntil} from "rxjs";
import jwt_decode from "jwt-decode";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {AuthenticationModel} from "../../shared/models/authentication.models";
import {StateService} from "../../shared/services/state/state.service";

@Component({
    selector: 'app-user-login',
    templateUrl: './user-login.component.html',
    styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
    private readonly destroyed = new Subject<void>();
    private isLoggingIn: boolean = false;
    private window: WindowProxy;

    constructor(@Inject(DOCUMENT) private document: Document,
                private authService: AuthenticationService,
                private stateService: StateService,
                private _router: Router,
                private _activatedRoute: ActivatedRoute) {
        this.window = document.defaultView!;

    }

    ngOnInit(): void {
        this._activatedRoute.queryParams.pipe(takeUntil(this.destroyed))
            .subscribe((params) => {
                if (params.code) {
                    this.logInWithAuthorizationCode(params.code)
                }
                if (params.error_description) {
                    // todo something with error
                    console.log(params.error_description);
                    this.reload();
                }
            })
    }

    ngOnDestroy() {
        this.destroyed.next();
        this.destroyed.complete();
    }


    authorize(provider: string) {
        const redirectUri = this.getRedirectUrl();
        this.authService.authorize(redirectUri, provider);
    }

    private logInWithAuthorizationCode(code: string) {
        if (!this.isLoggingIn) {
            this.isLoggingIn = true;

            const redirectUri = this.getRedirectUrl();

            this.authService.logInWithAuthorizationCode(code, redirectUri).subscribe({
                next: (response: AuthenticationModel) => {
                    const idTokenDecoded = jwt_decode<AuthenticationModel>(response.id_token);
                    response.role = idTokenDecoded.role;

                    this.authService.authenticationData = response;
                    this.stateService.userName = idTokenDecoded.name;

                    this.authService.sendAuthStateChangeNotification(true);

                    this._router.navigate([this.stateService.returnUrl]).then(r => {
                        this.stateService.returnUrl = "/";
                    })
                },
                error: (err: HttpErrorResponse) => {
                    this.isLoggingIn = false;
                    this.reload();
                }
            })
        }
    }

    private reload() {
        // remove error from queryParams
        this._router.navigate(['./']);
    }

    private getRedirectUrl(): string {
        return this.window.location.origin + this.window.location.pathname;
    }

}
