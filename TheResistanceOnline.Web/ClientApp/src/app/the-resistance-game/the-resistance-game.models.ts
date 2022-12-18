export interface JoinGameCommand {
  channelName: string;
}

export interface GameDetails {
  channelName: string;
  playersDetails: PlayerDetails[];
  isVoiceChannel: boolean;
  isAvailable: boolean;
  currentMissionRound: number;
  missionTeam: PlayerDetails[];
  missionSize: number;
  gameStage: GameStage;
  gameOptions: GameOptions;

  gameAction: GameAction;

  nextGameStage: GameStage;

  voteFailedCount: number;

  missionRounds: Map<number, boolean>;

  missionOutcome: boolean[];
}

export interface PlayerDetails {
  discordUserName?: string;
  userName?: string;
  resistanceTeamWins: number;
  spyTeamWins: number;
  isBot: boolean;
  isMissionLeader: boolean;
  playerId: string;
  team: TeamModel;
  selectedTeamMember: boolean;
  voted: boolean;
  approvedMissionTeam: boolean;

  continued: boolean;

  supportedMission: boolean;
  chose: boolean;
}

export interface GameDetailsResponse {
  errorMessage?: string;
  errorOccured: boolean;
  gameDetails: GameDetails;
}

export interface GameOptions {
  timeLimitMinutes: number;
  botCount: number;
}

export interface StartGameCommand {
  gameOptions: GameOptions;
}


export enum TeamModel {
  Resistance,
  Spy
}

export enum GameStage {
  GameStart,     // spies find out who the other spies are
  MissionPropose, // Mission Team Leader Selects the Mission Team
  Vote, // Everyone Votes on the proposed team for mission
  VoteResults, // Show the results from the most recent vote
  Mission, // If Vote Successful Mission Members go on mission
  MissionResults,// Show the results from the most recent mission
  GameOverSpiesWon,
  GameOverResistanceWon

}


export enum GameAction {
  None = -1,
  SubmitMissionPropose,
  SubmitVote,
  Continue,
  SubmitMissionChoice
}

export interface GameActionCommand {
  gameDetails: GameDetails,
}
