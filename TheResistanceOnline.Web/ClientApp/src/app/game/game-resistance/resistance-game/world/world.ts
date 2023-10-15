import {Scene} from "three";
import {Environment} from "./environment/environment";
import {Resources} from "../utils/resources";
import {Debug} from "../utils/debug";
import {Board} from "./board/board";
import {ResistanceGame} from "../resistance-game";
import {RayCasting} from "./raycasting";

export class World {
  private static instance: World;

  private readonly scene: Scene;
  private readonly environment: Environment;
  private readonly board: Board;
  // Utils
  private readonly resources: Resources;
  private readonly debug: Debug;
  private readonly raycaster: RayCasting;

  constructor() {
    const resistanceGame = new ResistanceGame()

    this.scene = resistanceGame.scene;

    this.resources = resistanceGame.resources;
    this.debug = resistanceGame.debug;

    // Add all children of World
    this.board = new Board();

    this.environment = new Environment();

    this.raycaster = new RayCasting();
  }

  update() {
  }

  destroy() {
    this.environment.destroy();
    this.board.destroy();
  }

  setPlayers(players: string[]) {
    this.board.createPlayerPieces(players);
  }

  setMissionLeader(player: string) {
    this.board.moveLeaderPiece(player);
  }

  setMissionBuildPhase(missionMembers: number) {
    const playerPieces = this.board.playerPieces?.map(p => p.playerPiece);
    if (playerPieces) {
      this.raycaster.objectsToTest = playerPieces;
    }
  }

}
