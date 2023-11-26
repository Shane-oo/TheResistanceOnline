import {ComponentRef, Injectable} from '@angular/core';
import {SwalContainerComponent} from './swal-container.component';
import {OverlayService} from '../overlay/overlay.service';
import Swal from 'sweetalert2';

export const enum SwalTypes {
  Info,
  Success,
  Warning,
  Error,

}

@Injectable()
export class SwalContainerService {


  private container!: ComponentRef<SwalContainerComponent>;
  private windowSizeIsSmall: boolean = false;

  constructor(private readonly overlayService: OverlayService) {
  }

  public showSwal(type: SwalTypes, message = "") {
    if (!this.container) {
      this.container = this.overlayService.addComponent(SwalContainerComponent);
    }
    // close a previous alert
    this.resetSwal();

    switch (type) {
      case SwalTypes.Error:
        this.container.instance.isError = true;
        if (message.length === 0) {
          message = "Error";
        }
        break;
      case SwalTypes.Success:
        this.container.instance.isSuccess = true;
        if (message.length === 0) {
          message = "Success";
        }
        break;
      case SwalTypes.Info:
        this.container.instance.isInfo = true;
        if (message.length === 0) {
          break; // message required for info
        }
        break;
      case SwalTypes.Warning:
        this.container.instance.isWarning = true;
        if (message.length === 0) {
          message = "Warning";
        }
        break;
    }
    this.container.instance.message = message;
    this.container.instance.isSwalVisible = true;
  }

  public fireNotifySpiesModal = (spies: string[]) => {
    this.checkSize();
    const formattedString = spies.join(', ');
    let htmlBody = `<div class="SpyFont">
                        <h4>Spies:</h4>
                        <div>
                            <p class=userList> ${formattedString} </p>
                        </div>`;
    if (!this.windowSizeIsSmall) {
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
    if (!this.windowSizeIsSmall) {
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
    this.container.instance.isInfo = false;
  }

  private checkSize() {
    this.windowSizeIsSmall = window.innerWidth < 760;
  }
}
