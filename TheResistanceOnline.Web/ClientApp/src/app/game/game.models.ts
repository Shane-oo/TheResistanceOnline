export interface StartGameModel {
  bots: number | null,
  botsAllowed: boolean,
  lobbyId: string,
  totalPlayers: number,
  userNames: string[],
  type: GameType
}

export interface StartGameCommand {
  bots: number | null,
  botsAllowed: boolean,
  lobbyId: string,
  totalPlayers: number,
  userNames: string[]
  type: GameType
}

export enum GameType {
  ResistanceClassic
}
