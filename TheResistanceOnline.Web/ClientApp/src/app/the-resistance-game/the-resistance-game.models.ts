export interface CreateGameCommand {
  lobbyName: string;
  createChatChannel: boolean;
  createVoiceChannel: boolean;
}

export interface JoinGameCommand{
  lobbyName:string;
}
export interface GameDetails {
  lobbyName: string;
  playersDetails: PlayerDetails[];
}

export interface PlayerDetails {

  profilePictureName: string;
  userName: string;
}
