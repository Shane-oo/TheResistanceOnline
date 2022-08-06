import { ComponentRef, Injectable } from '@angular/core';
import { SwalContainerComponent } from './swal-container.component';
import { OverlayService } from '../overlay/overlay.service';

@Injectable()
export class SwalContainerService {

  private container!: ComponentRef<SwalContainerComponent>;

  constructor(private readonly overlayService: OverlayService) {
  }

  public showSwal(message:string) {
    if(!this.container) {
      this.container = this.overlayService.addComponent(SwalContainerComponent);
    }
    this.container.instance.isSwalVisible = true;
    this.container.instance.message = message;
  }
}
