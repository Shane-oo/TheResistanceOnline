import { ComponentRef, Injectable } from '@angular/core';
import { SwalContainerComponent } from './swal-container.component';
import { OverlayService } from '../overlay/overlay.service';
import { DiscordLoginResponseModel } from '../../app/user/user.models';
import Swal, { SweetAlertResult } from 'sweetalert2';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';

export const enum SwalTypesModel {
  Info,
  Success,
  Warning,
  Error,
  // Custom Logic
  DiscordLoginRequested
}

@Injectable()
export class SwalContainerService {


  public discordLoginResponseChanged: Subject<DiscordLoginResponseModel> = new Subject<DiscordLoginResponseModel>();
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
        break;
    }
    this.container.instance.isSwalVisible = true;
  }

  public fireDiscordLoginRequested = () => {
    let idk = environment.Discord_Generated_URL;
    Swal.fire({
                title: 'hello',
                backdrop: false,
                showCancelButton: true,
                cancelButtonText: 'I Don\'t Want To Use Discord',
                html: `<a href=${environment.Discord_Generated_URL}>
                        Login With Discord
                       </a>`
              })
        .then((result: SweetAlertResult) => {
          if(result.isDenied || result.isDismissed) {
            let response: DiscordLoginResponseModel = {declinedLogin: true};
            this.discordLoginResponseChanged.next(response);
          } else {
          }
        });
  };
}

//  public showDiscordLoginRequested = () => {
//
//     // this.gameService.gameDetailsChanged.subscribe((value: GameDetails) => {
//     //   this.gameDetails = value;
//     //   this.userInGame = true;
//     // });
//     this.container.instance.discordLoginResponseChanged.subscribe((value: DiscordLoginResponseModel) => {
//       return value;
//     });
//   };
