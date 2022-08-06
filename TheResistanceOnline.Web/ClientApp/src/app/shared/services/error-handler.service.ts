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
                   let errorMessage = this.handleError(error);
                   return throwError(() => new Error(errorMessage));
                 })
               );
  }

  private handleError = (error: HttpErrorResponse): string => {
    if(error.status === 404) {
      return this.handleNotFound(error);
    } else if(error.status === 400) {
      this.handleBadRequest(error);
    }
    return '';
  };

  private handleNotFound = (error: HttpErrorResponse): string => {
    // Id rather send them to home page and display a swal error
    this.router.navigate(['/404']);
    return error.message;
  };

  private handleBadRequest = (error: HttpErrorResponse) => {
    if(this.router.url === '/user/register') {

      if(error.error.errors) {
        if(error.error.errors.ConfirmPassword) {
          this.swalService.showSwal(error.error.errors.ConfirmPassword, SwalTypesModel.Error);
        }
      } else {
        this.swalService.showSwal(error.error ? error.error : error.message, SwalTypesModel.Error);
      }
    }
  };
}
