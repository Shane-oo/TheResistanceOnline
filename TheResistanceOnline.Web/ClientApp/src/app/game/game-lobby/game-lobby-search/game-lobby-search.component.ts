import {Component, EventEmitter, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CreateLobbyCommand, CreateLobbyFormModel, SearchLobbyFormModel, SearchLobbyQuery} from "../../game.models";

@Component({
  selector: 'app-game-lobby-search',
  templateUrl: './game-lobby-search.component.html',
  styleUrls: ['./game-lobby-search.component.css']
})
export class GameLobbySearchComponent {
  public searchLobbyForm: FormGroup<SearchLobbyFormModel>;
  @Output() searchLobbyEvent = new EventEmitter<SearchLobbyQuery>();

  constructor() {
    this.searchLobbyForm = new FormGroup<SearchLobbyFormModel>({
      id: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required,
          Validators.maxLength(20),
          Validators.pattern("^[a-zA-Z0-9]+$"), // only allow text and numbers
        ]
      }),
    })
  }

  searchLobby(searchLobbyFormValue: any) {
    const formValues = {...searchLobbyFormValue};
    const searchLobbyQuery: SearchLobbyQuery = {
      id: formValues.id,
    };

    this.searchLobbyEvent.emit(searchLobbyQuery);
  }

  validateControl = (controlName: string) => {
    return this.searchLobbyForm.get(controlName)?.invalid;
  }

  hasError = (controlName: string, errorName: string) => {
    return this.searchLobbyForm.get(controlName)?.hasError(errorName);
  };
}
