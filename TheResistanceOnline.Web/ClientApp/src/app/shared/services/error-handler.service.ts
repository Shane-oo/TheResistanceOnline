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
    this.swalService.showSwal(SwalTypes.Error, errorMessage.description);
    return errorMessage;
  };


  private handleBadRequest = (httpErrorResponse: HttpErrorResponse): string => {
    const errorMessage = httpErrorResponse.error;
    if (errorMessage.type === "ValidationFailure") {
      const errorsByProperty: any = {};

      for (const validationError of errorMessage.errors) {
        const propertyName = validationError.propertyName;
        const errorMessageText = validationError.errorMessage;

        if (!errorsByProperty[propertyName]) {
          errorsByProperty[propertyName] = [];
        }

        errorsByProperty[propertyName].push(errorMessageText);
      }

      const concatenatedErrors = Object.keys(errorsByProperty)
        .map(propertyName => `${propertyName} - ${errorsByProperty[propertyName].join(', ')}`)
        .join('<br>');
      this.swalService.showSwal(SwalTypes.Error, concatenatedErrors);

    } else {
      this.swalService.showSwal(SwalTypes.Error, errorMessage.description);
    }

    return errorMessage;
  };

  private handleInternalServerError = (error: HttpErrorResponse): string => {
    const errorMessage = error.error;
    this.swalService.showSwal(SwalTypes.Error, errorMessage.description);
    return errorMessage;
  }
}
