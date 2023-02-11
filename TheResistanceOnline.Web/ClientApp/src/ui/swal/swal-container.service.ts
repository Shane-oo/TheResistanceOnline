import { ComponentRef, Injectable } from '@angular/core';
import { SwalContainerComponent } from './swal-container.component';
import { OverlayService } from '../overlay/overlay.service';
import { DiscordLoginResponseModel } from '../../app/user/user.models';
import Swal, { SweetAlertResult } from 'sweetalert2';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { PlayerDetails } from '../../app/the-resistance-game/the-resistance-game.models';

export const enum SwalTypesModel {
  Info,
  Success,
  Warning,
  Error,

}

@Injectable()
export class SwalContainerService {


  public discordLoginResponseChanged: Subject<DiscordLoginResponseModel> = new Subject<DiscordLoginResponseModel>();
  private container!: ComponentRef<SwalContainerComponent>;
  private windowSizeIsSmall: boolean = false;

  constructor(private readonly overlayService: OverlayService) {
  }

  public showSwal(message: string, type: SwalTypesModel) {
    if(!this.container) {
      this.container = this.overlayService.addComponent(SwalContainerComponent);
    }
    // close a previous alert
    this.resetSwal();
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
    this.checkSize();

    Swal.fire({
                title: 'Discord Social Login',
                backdrop: false,
                showCancelButton: true,
                cancelButtonText: 'I Don\'t Want To Use Discord',
                html: `<h6>Chat or talk with your friends throughout the game via our Discord Server Channels!</h6>
                       <a href=${environment.Discord_Generated_URL} class="discord-button">
                       <i class='fab fa-discord'></i> Login With Discord
                       </a>`,
                showConfirmButton: false
              })
        .then((result: SweetAlertResult) => {
          if(result.isDenied || result.isDismissed) {
            let response: DiscordLoginResponseModel = {declinedLogin: true};
            this.discordLoginResponseChanged.next(response);
          } else {
          }
        });
  };

  public fireNotifySpiesModal = (spies: PlayerDetails[]) => {
    this.checkSize();
    const formattedString = spies.map(p => p.userName).join(', ');
    let htmlBody = `<div class="SpyFont">
                        <h4>Spies:</h4>
                        <div>
                            <p class=userList> ${formattedString} </p>
                        </div>`;
    if(!this.windowSizeIsSmall) {
      htmlBody += `
                    <div>
                        <img class="leaderImage" alt="EvilSpyLeader" src="./assets/images/evil_goverment_lady.png">
                    </div>
                    <div class="css-typing">
                        <p>Together, complete your covert operation.</p>
                        <p> Sabotage the Resistances missions and remain unnoticed.</p>
                        <p>Do not fail your Government.</p>
                    </div>`;
    }
    htmlBody += `</div>`;

    Swal.fire({
                backdrop: false,
                background: '#1e1e1e',
                showCancelButton: false,
                html: htmlBody,
                showConfirmButton: true,
                confirmButtonText: 'Understood',
                showCloseButton: true,
                customClass: {
                  confirmButton: 'swalConfirmButtonSpy',
                  popup: 'swalModalSpy'
                }
              });
  };

  public fireNotifyResistanceModal = () => {
    this.checkSize();

    let htmlBody = `<div class="ResistanceFont">
                        <h4>Resistance!</h4>`;
    if(!this.windowSizeIsSmall) {
      htmlBody += `
                    <div>
                        <img class="leaderImage"  alt="ResistanceLeader" src="./assets/images/resistance_man_cyberpunk2.png">
                    </div>
                    <div class="css-typing-resistance">
                        <p>Complete your missions</p>
                        <p>Help Overthrow the corrupt Government</p>
                        <p>Beware of possible spies among us</p>
                    </div>`;
    }
    htmlBody += `</div>`;

    Swal.fire({
                backdrop: false,
                background: '#1e1e1e',
                showCancelButton: false,
                html: htmlBody,
                showConfirmButton: true,
                confirmButtonText: 'Understood',
                showCloseButton: true,
                customClass: {
                  confirmButton: 'swalConfirmButtonResistance',
                  popup: 'swalModalResistance'
                }
              });
  };

  private resetSwal() {
    this.container.instance.isSwalVisible = false;
    this.container.instance.isError = false;
    this.container.instance.isSuccess = false;
  }

  private checkSize(){
      this.windowSizeIsSmall = window.innerWidth < 760;
  }
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
