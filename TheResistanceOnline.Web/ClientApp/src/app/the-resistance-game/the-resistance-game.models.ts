export interface CreateGameCommand {
  lobbyName: string;
  createChatChannel: boolean;
  createVoiceChannel: boolean;
  userId: string;
}

export interface GameDetails {
  userInGame: boolean;
  lobbyName: string;
  users:string[];// string of usernames
}
