import {Scene} from "three";
import {Environment} from "./environment/environment";
import {Resources} from "../utils/resources";
import {Floor} from "./floor";
import {Fox} from "./fox";
import {Time} from "../utils/time";
import {Debug} from "../utils/debug";
import {Board} from "./board/board";
import {ResistanceGame} from "../resistance-game";

export class World {
  private static instance: World;

  private readonly scene: Scene;
  private readonly environment: Environment;
  private readonly board: Board;
  // Utils
  private readonly resources: Resources;
  private readonly debug: Debug;

  constructor() {
    const resistanceGame = new ResistanceGame()

    this.scene = resistanceGame.scene;
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
    this.board.destroy();
  }

  setPlayers(players: string[]) {
    this.board.createPlayerPieces(players);
  }

  setMissionLeader(player:string){
     this.board.moveLeaderPiece(player);
  }

}
