import { Component } from '@angular/core';

@Component({
             selector: 'swal-container',
             template: `
               <swal *ngIf="isSwalVisible"
                     text=" {{ message }}"
                     [swalFireOnInit]="true"
                     (didClose)="isSwalVisible = false"
                     titleText="poop"
                     width="600"
                     padding="3em"
                     backdrop="rgba(255,0,0,0.4)">
               </swal>
               <!--<swal *ngIf="!isSwalVisible"
                     text="modalFireCondition = {{ isSwalVisible }}"
                     [swalFireOnInit]="true"
                     (didClose)="isSwalVisible = false"
                     titleText="Big Massive poop"
                     width="600"
                     padding="3em"
                     backdrop="rgba(255,0,0,0.4)">
               </swal>-->
             `,
             providers: []

           })
export class SwalContainerComponent {

  public isSwalVisible: boolean = true;
  public message: string = '';

  constructor() {
  }

}
