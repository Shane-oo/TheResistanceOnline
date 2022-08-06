import { OverlayService } from './overlay.service';
import { Component, ComponentRef, Injector, Type, ViewContainerRef } from '@angular/core';

@Component({
             selector: 'overlay',
             template: ''
           })
export class OverlayComponent {
  constructor(private readonly viewContainer: ViewContainerRef,
              private readonly injector: Injector, private readonly overlayService: OverlayService) {
    this.overlayService.setOutlet(this);
  }

  addComponent<T>(componentType: Type<T>, data?: any): ComponentRef<T> {
    const providers = [{provide: 'data', useValue: data}];
    const injector = Injector.create({providers: providers, parent: this.injector});
    return this.viewContainer.createComponent(componentType, {injector: injector});
  }
}
