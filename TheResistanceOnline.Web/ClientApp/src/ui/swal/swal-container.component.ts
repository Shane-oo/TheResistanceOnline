import { Component } from '@angular/core';

@Component({
             selector: 'swal-container',
             template: `
               <!--Error Component-->
               <swal *ngIf="isSwalVisible && isError"
                     icon="error"
                     text=" {{ message }}"
                     [swalFireOnInit]="true"
                     (didClose)="isSwalVisible = false;isError = false"
                     titleText="Error">
               </swal>
               <!--Success Component-->
               <swal *ngIf="isSwalVisible && isSuccess"
                     icon="success"
                     text=" {{ message }}"
                     [swalFireOnInit]="true"
                     (didClose)="isSwalVisible = false;isSuccess = false"
                     titleText="Success">
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

}
