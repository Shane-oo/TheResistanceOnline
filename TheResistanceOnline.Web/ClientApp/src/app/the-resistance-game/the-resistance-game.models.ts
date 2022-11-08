export interface JoinGameCommand {
  channelName: string;
}

export interface GameDetails {
  channelName: string;
  playersDetails: PlayerDetails[];
  isVoiceChannel: boolean;
  isAvailable: boolean;
}

export interface PlayerDetails {
  discordUserName?: string;
  userName: string;
  resistanceTeamWins: number;
  spyTeamWins: number;
  isBot: boolean;
  isMissionLeader: boolean;
  playerId: string;
  team: TeamModel;
}

export interface GameDetailsResponse {
  errorMessage?: string;
  errorOccured: boolean;
  gameDetails: GameDetails;
}

export interface GameOptions {
  timeLimitMinutes: number;
  moveTimeLimitMinutes: number;
  botCount: number;
}

export interface StartGameCommand {
  gameOptions: GameOptions;
}


enum TeamModel {
  Resistance,
  Spy
}
