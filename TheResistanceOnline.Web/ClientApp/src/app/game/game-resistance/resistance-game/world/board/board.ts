import {Group, Mesh, Scene} from "three";
import {ModelResource, Resources} from "../../utils/resources";
import GUI from "lil-gui";
import {PlayerPiece} from "./player-piece";
import {ResistanceGame} from "../../resistance-game";
import {Sizes} from "../../utils/sizes";

export class Board {

  private readonly playerPositions: { x: number, z: number }[] = [
    {x: -2, z: -1}, // top left
    {x: 0, z: -1}, // top middle
    {x: 2, z: -1}, // top right

    {x: 2.75, z: 0.5}, // right top
    {x: 2.75, z: -0.5}, // right bottom

    {x: 2, z: 1}, // bottom right
    {x: 0, z: 1}, // bottom middle
    {x: -2, z: 1}, // bottom left

    {x: -2.75, z: 0.5}, // left bottom
    {x: -2.75, z: -0.5} // left top
  ];

  private readonly scene: Scene;
  private readonly modelResource: ModelResource;
  private readonly model: Group;
  private playerPieces?: PlayerPiece[];
  // Utils
  private readonly resources: Resources;
  private readonly sizes: Sizes;
  // Debug
  private readonly debugFolder?: GUI;

  constructor() {
    const resistanceGame = new ResistanceGame();

    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;
    this.sizes = resistanceGame.sizes;

    // Board
    this.modelResource = this.resources.getModelResourceByName('resistanceGameBoard');
    this.model = this.modelResource.gltf.scene;
    this.configureModel();
    this.scene.add(this.model);


    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder('board');

      this.configureDebug();
    }
  }

  createPlayerPieces(players: string[]) {
    const playerPieces: PlayerPiece[] = [];

    for (let i = 0; i < players.length; i++) {
      const piece = new PlayerPiece(players[i], this.playerPositions[i]);
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
    this.model.scale.set(0.143, 0.05, 0.143);
    this.model.traverse((child) => {
      if (child instanceof Mesh) {
        child.castShadow = true;
      }
    });
  }

  private configureDebug() {
    if (this.debugFolder) {
      this.debugFolder.add(this.model.scale, 'x')
        .name('scaleX')
        .min(0)
        .max(2)
        .step(0.001)
      this.debugFolder.add(this.model.scale, 'y')
        .name('scaley')
        .min(0)
        .max(2)
        .step(0.001)
      this.debugFolder.add(this.model.scale, 'z')
        .name('scalez')
        .min(0)
        .max(2)
        .step(0.001)
    }
  }

}
