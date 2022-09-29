import { Component } from '@angular/core';
import { DidOpenEvent } from '@sweetalert2/ngx-sweetalert2';
import Swal from 'sweetalert2';

@Component({
             selector: 'swal-container',
             template: `
               <!--Error Component-->
               <swal *ngIf="isSwalVisible && isError"
                     [toast]="true"
                     position='top-end'
                     [showConfirmButton]="false"
                     [timer]="7500"
                     icon="error"
                     html="<h6 class=swalError>{{ message }}</h6>"
                     [swalFireOnInit]="true"
                     (didClose)="isSwalVisible = false;isError = false"
                     [backdrop]="false"
                     background='#383838'
                     (didOpen)="swalDidOpen($event)">

               </swal>
               <!--Success Component-->
               <swal *ngIf="isSwalVisible && isSuccess"
                     icon="success"
                     text=" {{ message }}"
                     [swalFireOnInit]="true"
                     (didClose)="isSwalVisible = false;isSuccess = false"
                     titleText="Success"
                     [backdrop]="false"
               >
               </swal>
             `,
             providers: []

           })
export class SwalContainerComponent {

  public isSwalVisible: boolean = false;
  public message: string = '';
  public isError: boolean = false;
  public isSuccess: boolean = false;

  constructor() {
  }

  public swalDidOpen(event: DidOpenEvent): void {
    // Most events (those using $event in the example above) will let you access the modal native DOM node, like this:
    console.log(event.modalElement);
    event.modalElement.addEventListener('mouseenter', Swal.stopTimer);
    event.modalElement.addEventListener('mouseleave', Swal.resumeTimer);
  }

}
