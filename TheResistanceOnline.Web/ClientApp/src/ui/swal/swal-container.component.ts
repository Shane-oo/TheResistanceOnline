import {Component} from '@angular/core';
import {DidOpenEvent} from '@sweetalert2/ngx-sweetalert2';
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
            html="<span class=swal-text>{{ message }}</span>"
            [swalFireOnInit]="true"
            (didClose)="isSwalVisible = false;isError = false"
            background='#271807'
            (didOpen)="swalDidOpen($event)"
      >
      </swal>
      <!--Success Component-->
      <swal *ngIf="isSwalVisible && isSuccess"
            [toast]="true"
            position='top-end'
            [showConfirmButton]="false"
            [timer]="2000"
            icon="success"
            html="<span class=swal-text>{{ message }}</span>"
            [swalFireOnInit]="true"
            (didClose)="isSwalVisible = false;isSuccess = false"
            background='#271807'
            (didOpen)="swalDidOpen($event)">
      </swal>
      <!--Warning Component todo-->
      <!--Info Component -->
      <swal *ngIf="isSwalVisible && isInfo"
            [toast]="true"
            position='top-end'
            [showConfirmButton]="false"
            [timer]="2000"
            icon="info"
            html="<span class=swal-text>{{ message }}</span>"
            [swalFireOnInit]="true"
            (didClose)="isSwalVisible = false;isInfo = false"
            background='#271807'
            (didOpen)="swalDidOpen($event)">
      </swal>
  `,
  providers: []

})
export class SwalContainerComponent {

  public isSwalVisible: boolean = false;
  public message: string = '';
  public isError: boolean = false;
  public isSuccess: boolean = false;
  public isInfo: boolean = false;

  constructor() {
  }

  public swalDidOpen(event: DidOpenEvent): void {
    event.modalElement.addEventListener('mouseenter', Swal.stopTimer);
    event.modalElement.addEventListener('mouseleave', Swal.resumeTimer);
  }

}
