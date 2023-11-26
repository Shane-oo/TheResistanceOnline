import {FormControl} from "@angular/forms";

export interface CreateLobbyCommand {
  id: string,
  isPrivate: boolean,
  maxPlayers: number,
  fillWithBots: boolean
}

export interface CreateLobbyFormModel {
  id: FormControl<string>,
  isPrivate: FormControl<boolean>,
  maxPlayers: FormControl<number>,
  fillWithBots: FormControl<boolean>
}

export interface JoinLobbyCommand {
  lobbyId: string
}

export interface SearchLobbyFormModel {
  id: FormControl<string>
}

export interface LobbyDetails {
  id: string,
  isPrivate: boolean,
  maxPlayers: number,
  fillWithBots: boolean,
  connections: ConnectionModel[]
}

export interface ConnectionModel {
  connectionId: string,
  userName: string,
  isReady: boolean
}

export interface ReadyUpCommand {
  lobbyId: string;
}
