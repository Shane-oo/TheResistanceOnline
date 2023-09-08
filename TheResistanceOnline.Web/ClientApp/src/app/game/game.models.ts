import {FormControl} from "@angular/forms";

export interface CreateLobbyCommand {
  roomId: string,
  privateRoom: boolean
}

export interface CreateLobbyFormModel {
  roomId: FormControl<string>,
  privateRoom: FormControl<boolean>
}
