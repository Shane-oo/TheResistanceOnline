import {FormControl} from "@angular/forms";

export interface CreateLobbyCommand {
  id: string,
  isPrivate: boolean
}

export interface CreateLobbyFormModel {
  id: FormControl<string>,
  isPrivate: FormControl<boolean>
}

export interface JoinLobbyCommand {
  lobbyId: string
}

export interface SearchLobbyQuery {
  id: string
}

export interface SearchLobbyFormModel {
  id: FormControl<string>
}

export interface LobbyDetails {
  id: string,
  isPrivate: boolean,
  connections: ConnectionModel[]
}

export interface ConnectionModel {
  connectionId: string,
  userName: string
}
