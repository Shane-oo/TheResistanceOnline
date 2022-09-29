import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';

@Injectable({
              providedIn: 'root'
            })

export class ErrorHandlerService implements HttpInterceptor {

  constructor(private router: Router, private swalService: SwalContainerService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
               .pipe(
                 catchError((error: HttpErrorResponse) => {
                   this.handleError(error);
                   // ToDo this should be different but idk
                   return throwError(() => new Error());
                 })
               );
  }

  private handleError = (error: HttpErrorResponse): void => {
    if(error.status === 404) {
      this.handleNotFound(error);
    } else if(error.status === 400) {
      this.handleBadRequest(error);
    } else if(error.status === 401) {
      this.handleUnauthorized(error);
    } else if(error.status === 403) {
      this.handleForbidden(error);
    }

  };

  private handleNotFound = (error: HttpErrorResponse): string => {
    // Id rather send them to home page and display a swal error
    console.log('handle not found');
    //this.router.navigate(['/404']);
    return error.message;
  };

  private handleBadRequest = (error: HttpErrorResponse) => {
    if(error.error.errors) {
      if(error.error.errors.ConfirmPassword) {
        this.swalService.showSwal(error.error.errors.ConfirmPassword, SwalTypesModel.Error);
        return;
      }
    }
    this.swalService.showSwal(error.error ? error.error : error.message, SwalTypesModel.Error);
    return;
  };

  private handleUnauthorized = (error: HttpErrorResponse) => {
    this.swalService.showSwal('Unauthorized: ' + error.error ? error.error : error.message, SwalTypesModel.Error);
  };
  private handleForbidden = (error: HttpErrorResponse) => {
    this.swalService.showSwal('Forbidden', SwalTypesModel.Error);
  };

}
