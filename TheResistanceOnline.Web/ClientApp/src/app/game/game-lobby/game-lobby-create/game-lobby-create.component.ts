import {Component, EventEmitter, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CreateLobbyCommand, CreateLobbyFormModel} from "../../game.models";
import {UpdateUserCommand} from "../../../user/user.models";

@Component({
  selector: 'app-game-lobby-create',
  templateUrl: './game-lobby-create.component.html',
  styleUrls: ['./game-lobby-create.component.css']
})
export class GameLobbyCreateComponent {
  public createLobbyForm: FormGroup<CreateLobbyFormModel>;
  @Output() createLobbyEvent = new EventEmitter<CreateLobbyCommand>();

  constructor() {
    this.createLobbyForm = new FormGroup<CreateLobbyFormModel>({
      roomId: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required,
          Validators.maxLength(12),
          Validators.pattern('^[A-Za-z0-9ñÑáéíóúÁÉÍÓÚ/^\S*$/]+$'), // only allow text and numbers
        ]
      }),
      privateRoom: new FormControl(false, {
        nonNullable: true,
        validators: [Validators.required]
      })
    })
  }

  createLobby(createLobbyFormValue: any) {
    const formValues = {...createLobbyFormValue};
    const createLobbyCommand: CreateLobbyCommand = {
      roomId: formValues.roomId,
      privateRoom: formValues.privateRoom
    };

    this.createLobbyEvent.emit(createLobbyCommand);
  }

  validateControl = (controlName: string) => {
    return this.createLobbyForm.get(controlName)?.invalid;
  }

  hasError = (controlName: string, errorName: string) => {
    return this.createLobbyForm.get(controlName)?.hasError(errorName);
  };
}
