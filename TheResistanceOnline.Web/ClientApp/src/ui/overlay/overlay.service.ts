import { ComponentRef, Injectable, Type } from '@angular/core';
import { OverlayComponent } from './overlay.component';

@Injectable()
export class OverlayService {
  private outlet!: OverlayComponent;

  public addComponent<T>(componentType: Type<T>, data: any = null): ComponentRef<T> {
    return this.outlet.addComponent(componentType, data);
  }

  public setOutlet(outlet: OverlayComponent) {
    this.outlet = outlet;
  }
}
