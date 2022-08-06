import { Injectable, ViewChild } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { SwalContainerService } from '../../../ui/swal/swal-container.service';

@Injectable({
              providedIn: 'root'
            })

export class ErrorHandlerService implements HttpInterceptor {

  constructor(private router: Router,private swalService:SwalContainerService) {
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

  private handleError = (error:HttpErrorResponse):string=>{
    if(error.status === 404){
      return this.handleNotFound(error);
    }
    else if(error.status === 400){
      return this.handleBadRequest(error);
    }
    return "";
  }

   private handleNotFound= (error:HttpErrorResponse):string=>{
    this.router.navigate(['/404']);
    return error.message;
   }

  private handleBadRequest = (error: HttpErrorResponse): string => {
    if(this.router.url === '/user/register'){
      console.log("error occured in register")
      this.swalService.showSwal(error.error ? error.error : error.message);

      return error.error ? error.error : error.message;
    }
    else{
      return error.error ? error.error : error.message;
    }
  }

}
