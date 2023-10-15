import {Injectable} from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError} from 'rxjs';
import {AuthenticationService} from "./authentication.service";
import {AuthenticationModel} from "../../models/authentication.models";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
    private isRefreshingToken = false;
    private tokenSubject = new BehaviorSubject<string>("");

    constructor(private readonly authService: AuthenticationService) {
    }

    // Intercepts all http calls and if access token exists will attach token to request
    // if 401 is returned then refresh token is attempted and original request is resent if refresh token successful
    // if 403 is returned then user is logged out
    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

        const authentication = this.authService.authenticationData;
        if (authentication?.access_token) {
            return next.handle(this.addTokenToRequest(request, authentication?.access_token))
                .pipe(
                    catchError((error: HttpErrorResponse) => {
                        if (error.status === 401) {
                            return this.handleUnauthorizedError(request, next);
                        } else if (error.status === 403) {
                            this.authService.logOut();
                        }

                        return throwError(() => error);
                    })
                )
        }

        return next.handle(request);
    }


    private addTokenToRequest = (req: HttpRequest<any>, accessToken: string): HttpRequest<unknown> => {
        return req.clone({setHeaders: {Authorization: `Bearer ${accessToken}`}});
    }

    // Try refreshing token and if refresh token successful resend original request
    // If refresh token unsuccessful logout user
    private handleUnauthorizedError(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        if (!this.isRefreshingToken) {
            this.isRefreshingToken = true;
            this.tokenSubject.next("");

            this.authService.refreshToken().subscribe({
                next: (response: AuthenticationModel) => {
                    this.isRefreshingToken = false;

                    this.tokenSubject.next(response.access_token);
                    // Update tokens
                    let authorizationModel = this.authService.authenticationData;
                    if (authorizationModel) {
                        authorizationModel.access_token = response.access_token;
                        authorizationModel.refresh_token = response.refresh_token;
                        authorizationModel.id_token = response.id_token;
                        authorizationModel.expires_in = response.expires_in;

                        this.authService.authenticationData = authorizationModel;

                        return next.handle(this.addTokenToRequest(request, authorizationModel.access_token));
                    }
                    return throwError(() => new Error(''));
                },
                error: (err: HttpErrorResponse) => {
                    this.authService.logOut();
                    this.isRefreshingToken = false;
                    return throwError(() => err);
                }
            });
        }
        return this.tokenSubject
            .pipe(filter(token => token != ""),
                take(1),
                switchMap(token => next.handle(this.addTokenToRequest(request, token))));
    }
}
