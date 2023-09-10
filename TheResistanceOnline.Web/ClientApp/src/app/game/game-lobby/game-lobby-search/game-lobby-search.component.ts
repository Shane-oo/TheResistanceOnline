import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CreateLobbyCommand, CreateLobbyFormModel, SearchLobbyFormModel, SearchLobbyQuery} from "../../game.models";

@Component({
  selector: 'app-game-lobby-search',
  templateUrl: './game-lobby-search.component.html',
  styleUrls: ['./game-lobby-search.component.css']
})
export class GameLobbySearchComponent {
  public searchLobbyForm: FormGroup<SearchLobbyFormModel>;

  constructor() {
    this.searchLobbyForm = new FormGroup<SearchLobbyFormModel>({
      id: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required,
          Validators.maxLength(20),
          Validators.pattern('^[A-Za-z0-9ñÑáéíóúÁÉÍÓÚ/^\S*$/]+$'), // only allow text and numbers
        ]
      }),
    })
  }

  searchLobby(searchLobbyFormValue: any) {
    const formValues = {...searchLobbyFormValue};
    const searchLobbyQuery: SearchLobbyQuery = {
      id: formValues.id,
    };
    console.log("find ",searchLobbyQuery)

    //this.createLobbyEvent.emit(createLobbyCommand);
  }

  validateControl = (controlName: string) => {
    return this.searchLobbyForm.get(controlName)?.invalid;
  }

  hasError = (controlName: string, errorName: string) => {
    return this.searchLobbyForm.get(controlName)?.hasError(errorName);
  };
}
