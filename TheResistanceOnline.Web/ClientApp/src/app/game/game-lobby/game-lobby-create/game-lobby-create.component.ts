import {Component, EventEmitter, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CreateLobbyCommand, CreateLobbyFormModel} from "../game-lobby.models";


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
      id: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required,
          Validators.maxLength(20),
          Validators.pattern("^[a-zA-Z0-9]+$"), // only allow text and numbers
        ]
      }),
      isPrivate: new FormControl(false, {
        nonNullable: true,
        validators: [Validators.required]
      }),
      maxPlayers: new FormControl(10, {
        nonNullable: true,
        validators: [Validators.required,
          Validators.max(10),
          Validators.min(5)
        ]
      }),
      fillWithBots: new FormControl(true, {
        nonNullable: true,
        validators: [Validators.required]
      })
    })
  }

  createLobby(createLobbyFormValue: any) {
    const formValues = {...createLobbyFormValue};
    const createLobbyCommand: CreateLobbyCommand = {
      id: formValues.id,
      isPrivate: formValues.isPrivate,
      maxPlayers: formValues.maxPlayers,
      fillWithBots: formValues.fillWithBots
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
