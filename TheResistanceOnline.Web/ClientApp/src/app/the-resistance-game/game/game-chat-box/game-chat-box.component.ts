import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faMessage } from '@fortawesome/free-solid-svg-icons';
import { TheResistanceGameService } from '../../the-resistance-game.service';
import { Message } from '../../the-resistance-game.models';
import { Subject, takeUntil } from 'rxjs';

@Component({
             selector: 'app-game-chat-box',
             templateUrl: './game-chat-box.component.html',
             styleUrls: ['./game-chat-box.component.css']
           })
export class GameChatBoxComponent {
  @ViewChild('containerElement') containerElement!: ElementRef;

  public messageIcon = faMessage;
  public messageForm: FormGroup = new FormGroup({});
  public messages: Message[] = [];
  public nameToColorMap: Map<string, string> = new Map<string, string>();
  private readonly destroyed = new Subject<void>();

  constructor(private gameService: TheResistanceGameService) {
    this.gameService.messageReceived
        .pipe(takeUntil(this.destroyed))
        .subscribe((value: Message) => {
          if(!this.nameToColorMap.has(value.name)) {
            this.nameToColorMap.set(value.name, this.generateBrightHexColor());
          }
          this.messages.push(value);
        });

    this.gameService.addReceiveMessageListener();
  }

  ngOnInit(): void {
    this.messageForm = new FormGroup({
                                       message: new FormControl('', [Validators.required, Validators.maxLength(150)])
                                     });
  }

  ngAfterViewInit() {
    // initialize containerElement property here
    this.containerElement.nativeElement.scrollTop = this.containerElement.nativeElement.scrollHeight;
  }

  ngOnDestroy() {
    this.destroyed.next();
    this.destroyed.complete();
    this.gameService.removeReceiveMessageListener();
  }


  //UserRegisterModel
  sendMessage = (sendMessageFormValue: any) => {
    // send message to hub
    this.gameService.sendMessage(sendMessageFormValue.message);
    this.messageForm.reset();
    this.containerElement.nativeElement.scrollTop = this.containerElement.nativeElement.scrollHeight;

  };

  private generateBrightHexColor(): string {
    // Generate a random number between 0 and 255 for each RGB value
    const red = Math.floor(Math.random() * 256);
    const green = Math.floor(Math.random() * 256);
    const blue = Math.floor(Math.random() * 256);

    // Convert the RGB values to a hex string
    const hex = '#' + ((1 << 24) + (red << 16) + (green << 8) + blue).toString(16).slice(1);

    // Check if the hex color is bright enough
    const brightnessThreshold = 150; // Adjust this value to change the brightness threshold
    const brightness = (red * 299 + green * 587 + blue * 114) / 1000;
    if(brightness < brightnessThreshold) {
      return this.generateBrightHexColor(); // If the color is too dark, generate a new color
    }

    return hex;
  }
}
