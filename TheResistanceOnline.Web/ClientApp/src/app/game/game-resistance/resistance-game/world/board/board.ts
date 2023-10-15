import {Group, Mesh, Scene} from "three";
import {ModelResource, Resources} from "../../utils/resources";
import GUI from "lil-gui";
import {PlayerPiece} from "./player-piece";
import {ResistanceGame} from "../../resistance-game";

export class Board {
  private readonly scene: Scene;
  private readonly modelResource: ModelResource;
  private readonly model: Group;
  private playerPieces?: PlayerPiece[];
  // Utils
  private readonly resources: Resources;
  // Debug
  private readonly debugFolder?: GUI;

  constructor() {
    const resistanceGame = new ResistanceGame();

    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;

    // Board
    this.modelResource = this.resources.getModelResourceByName('resistanceGameBoard');
    this.model = this.modelResource.gltf.scene;
    this.configureModel();
    this.scene.add(this.model);


    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder('board');
    }
  }

  createPlayerPieces(players: string[]) {
    const playerPieces: PlayerPiece[] = [];

    for (const player of players) {
      const piece = new PlayerPiece(player);
      playerPieces.push(piece);
    }

    this.playerPieces = playerPieces;
  }

  destroy() {
    if (this.playerPieces) {
      for (const piece of this.playerPieces) {
        piece.destroy();
      }
    }
  }

  private configureModel() {
    this.model.scale.set(0.2, 0.2, 0.2);
    this.model.traverse((child) => {
      if (child instanceof Mesh) {
        child.castShadow = true;
      }
    });
  }
}
