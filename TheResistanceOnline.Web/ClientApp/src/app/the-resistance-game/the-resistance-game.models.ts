export interface CreateGameCommand {
  lobbyName: string;
  createChatChannel: boolean;
  createVoiceChannel: boolean;
  userId: string;
}

export interface GameDetails {
  userInGame: boolean;
  lobbyName: string;
  playersDetails: PlayerDetails[];
}

export interface PlayerDetails {

  profilePictureName: string;
  userName: string;
}
