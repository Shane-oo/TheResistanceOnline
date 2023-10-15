import {BoxGeometry, BufferGeometry, Material, Mesh, MeshStandardMaterial, Scene} from "three";
import GUI from "lil-gui";
import {ResistanceGame} from "../../resistance-game";
import {Resources} from "../../utils/resources";

// there will be multiple player pieces, one for each player
export class PlayerPiece {
  private readonly scene: Scene;
  private readonly mesh: Mesh<BufferGeometry, Material>;
  private readonly name: string;
  // Utils
  private readonly resources: Resources;
  // Debug
  private readonly debugFolder?: GUI;

  constructor(name: string) {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;

    this.name = name;
    this.mesh = this.createPiece();
    this.scene.add(this.mesh);

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder(`player-piece-${name}`); // add this to a folder of player pieces todo
    }
  }

  destroy() {

  }

  private createPiece(): Mesh<BufferGeometry, Material> {
    const material = new MeshStandardMaterial({color: 'blue'});
    const geometry = new BoxGeometry(2, 2, 2);
    return new Mesh<BufferGeometry, Material>(geometry, material);
  }
}
