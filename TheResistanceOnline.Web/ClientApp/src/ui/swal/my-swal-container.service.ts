import { ComponentRef, Injectable, Injector, ViewContainerRef } from '@angular/core';
import { SwalContainerComponent } from './swal-container.component';

@Injectable()
export class MySwalContainerService{

  private container! :ComponentRef<SwalContainerComponent>;

  constructor( private viewContainer:ViewContainerRef,private readonly injector:Injector) {
  }
  public showError(){
    if(!this.container) {
      const providers = [{provide:'data',useValue:null}]   ;
      const injector = Injector.create({providers:providers,parent:this.injector});
      this.container = this.viewContainer.createComponent(SwalContainerComponent,{injector:injector});
    }
    this.container.instance.isSwalVisible = true;
  }
}
