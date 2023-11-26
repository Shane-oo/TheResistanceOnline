import {BufferGeometry, Mesh, MeshStandardMaterial, Scene, Vector3} from "three";
import {ResistanceGame} from "../../../resistance-game";
import {Resources} from "../../../utils/resources";
import GUI from "lil-gui";

export abstract class Piece {
  readonly name: string;
  readonly mesh: Mesh<BufferGeometry, MeshStandardMaterial>;

  readonly scene: Scene;
  // Utils
  readonly resources: Resources;
  // Debug
  readonly debugFolder?: GUI;

  protected constructor(name: string) {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;

    this.name = name;
    this.mesh = this.createMesh();
    this.scene.add(this.mesh);

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder(name);
    }
  }

  abstract createMesh(): Mesh<BufferGeometry, MeshStandardMaterial>;

  movePiece(position: Vector3) {
    this.mesh.position.setX(position.x);
    this.mesh.position.setZ(position.z);
  }

  destroy() {
    this.mesh.geometry.dispose();
    this.mesh.material.dispose();
  }

}
