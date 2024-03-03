export interface CommenceGameModel {
  isMissionLeader: boolean;
  missionLeader: string;
  phase: Phase;
  players: string[];
  team: Team;
  teamMates: string[] | null;
}

export enum Phase {
  MissionBuild,
  Vote,
  VoteResults,
  Mission,
  MissionResults
}

export enum Team {
  Resistance,
  Spy
}


export interface VoteResultsModel {
  playerNameToVoteApproved: Map<string, boolean>;
  voteSuccessful: boolean;
}
