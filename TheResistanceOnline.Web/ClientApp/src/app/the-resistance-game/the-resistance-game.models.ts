export interface CreateGameCommand {
  lobbyName: string;
  createChatChannel: boolean;
  createVoiceChannel: boolean;
}

export interface JoinGameCommand {
  lobbyName: string;
}

export interface GameDetails {
  channelName: string;
  playersDetails: PlayerDetails[];
  isVoiceChannel: boolean;
  isAvailable: boolean;
}

export interface PlayerDetails {
  discordUserName: string;
  profilePictureName: string;
  userName: string;
}
