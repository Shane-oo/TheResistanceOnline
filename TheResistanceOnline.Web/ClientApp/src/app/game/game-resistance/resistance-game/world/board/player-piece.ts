import {BoxGeometry, BufferGeometry, Material, Mesh, MeshStandardMaterial, Scene} from "three";
import GUI from "lil-gui";
import {ResistanceGame} from "../../resistance-game";
import {Resources} from "../../utils/resources";

// there will be multiple player pieces, one for each player
export class PlayerPiece {
  private readonly scene: Scene;
  private readonly mesh: Mesh<BufferGeometry, Material>;
  private readonly name: string;
  private readonly position: { x: number, z: number, };
  // Utils
  private readonly resources: Resources;
  // Debug
  private readonly debugFolder?: GUI;

  constructor(name: string, position: { x: number, z: number }) {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;

    this.name = name;
    this.position = position;
    this.mesh = this.createPiece();
    this.scene.add(this.mesh);

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder(`player-piece-${name}`);
      this.configureDebug();
    }
  }

  destroy() {

  }

  private createPiece(): Mesh<BufferGeometry, Material> {
    const material = new MeshStandardMaterial({color: 'blue'});
    const geometry = new BoxGeometry(0.25, 0.25, 0.25);

    const mesh = new Mesh<BufferGeometry, Material>(geometry, material);
    mesh.position.setX(this.position.x);
    mesh.position.setZ(this.position.z);

    console.log(mesh.position)
    return mesh;
  }

  private configureDebug() {
    this.debugFolder?.add(this.mesh.position, 'x')
      .name('PositionX')
      .min(-10)
      .max(10)
      .step(0.1);
    this.debugFolder?.add(this.mesh.position, 'y')
      .name('PositionY')
      .min(-10)
      .max(10)
      .step(0.1);
    this.debugFolder?.add(this.mesh.position, 'z')
      .name('PositionZ')
      .min(-10)
      .max(10)
      .step(0.1);
  }

}
