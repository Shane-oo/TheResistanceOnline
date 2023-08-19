import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {SwalContainerService, SwalTypes} from '../../../ui/swal/swal-container.service';

@Injectable({
  providedIn: 'root'
})

export class ErrorHandlerService implements HttpInterceptor {

  constructor(private swalService: SwalContainerService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = this.handleError(error);
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  private handleError = (error: HttpErrorResponse): string => {
    if (error.status === 404) {
      return this.handleNotFound(error);
    } else if (error.status === 400) {
      return this.handleBadRequest(error);
    } else if (error.status === 500) {
      return this.handleInternalServerError(error);
    }

    return "";
  };

  private handleNotFound = (error: HttpErrorResponse): string => {
    const errorMessage = error.error;
    this.swalService.showSwal(errorMessage, SwalTypes.Error);
    return errorMessage;
  };

  private handleBadRequest = (error: HttpErrorResponse): string => {
    const errorMessage = error.error;
    this.swalService.showSwal(errorMessage, SwalTypes.Error);
    return errorMessage;
  };

  private handleInternalServerError = (error: HttpErrorResponse): string => {
    const errorMessage = error.error.detail;
    this.swalService.showSwal(errorMessage, SwalTypes.Error);
    return errorMessage;
  }
}
