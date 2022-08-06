import { ComponentRef, Injectable } from '@angular/core';
import { SwalContainerComponent } from './swal-container.component';
import { OverlayService } from '../overlay/overlay.service';

export const enum SwalTypesModel {
  Info,
  Success,
  Warning,
  Error
}

@Injectable()
export class SwalContainerService {


  private container!: ComponentRef<SwalContainerComponent>;

  constructor(private readonly overlayService: OverlayService) {
  }

  public showSwal(message: string, type: SwalTypesModel) {
    if(!this.container) {
      this.container = this.overlayService.addComponent(SwalContainerComponent);
    }
    this.container.instance.message = message;

    switch(type) {
      case SwalTypesModel.Error:
        this.container.instance.isError = true;
        break;
      case SwalTypesModel.Success:
        this.container.instance.isSuccess = true;
    }
    this.container.instance.isSwalVisible = true;
  }
}
