import {Scene} from "three";
import {Environment} from "./environment/environment";
import {Resources} from "../utils/resources";
import {Debug} from "../utils/debug";
import {Board} from "./board/board";
import {ResistanceGame} from "../resistance-game";
import {RayCasting} from "./raycasting";
import {takeUntil} from "rxjs";

export class World {

  private readonly scene: Scene;
  private readonly environment: Environment;
  private readonly board: Board;
  // Utils
  private readonly resources: Resources;
  private readonly debug: Debug;
  private readonly rayCasting: RayCasting;

  constructor() {
    const resistanceGame = new ResistanceGame()

    this.scene = resistanceGame.scene;

    this.rayCasting = resistanceGame.rayCasting;
    this.resources = resistanceGame.resources;
    this.debug = resistanceGame.debug;

    // Add all children of World
    this.board = new Board();

    this.environment = new Environment();

  }

  update() {
  }

  destroy() {
    this.environment.destroy();
    this.rayCasting.destroy();
    this.board.destroy();
  }

  setPlayers(players: string[]) {
    this.board.createPlayerPieces(players);
  }

  setMissionLeader(player: string) {
    this.board.moveLeaderPiece(player);
  }

  addMissionTeamMember(player: string){
    this.board.addMissionTeamMemberPieceToPlayer(player);
  }

  removeMissionTeamMember(player:string){
    this.board.removeMissionTeamMemberPieceFromPlayer(player);
  }

  setMissionBuildPhase(missionMembers: number) {
    const playerPieces = this.board.playerPieces?.map(p => p.mesh);
    if (playerPieces) {
      this.rayCasting.objectsToTest = playerPieces;
    }

  }

}
