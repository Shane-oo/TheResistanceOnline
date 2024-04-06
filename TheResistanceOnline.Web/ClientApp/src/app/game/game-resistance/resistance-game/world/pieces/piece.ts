import {BufferGeometry, Color, Mesh, MeshStandardMaterial, Scene, Vector3} from "three";
import {ResistanceGame} from "../../resistance-game";
import {Resources} from "../../utils/resources";
import GUI from "lil-gui";
import {Dispose} from "../../utils/dispose";

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
    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder(name);
      this.debugFolder.close();
    }

    this.name = name;
    this.mesh = this.createMesh();
    this.mesh.name = this.name;

    this.scene.add(this.mesh);
  }

  abstract createMesh(): Mesh<BufferGeometry, MeshStandardMaterial>;

  movePiece(position: Vector3) {
    this.mesh.position.set(position.x, position.y, position.z);
  }

  lookAt(position: Vector3) {
    this.mesh.lookAt(position);
  }

  destroy() {
    this.mesh.geometry?.dispose();
    this.mesh.material?.dispose();

    this.mesh.traverse((child) => {
      Dispose.disposeOfChild(child)
    })
  }

  setVisible(visible: boolean): void {
    this.mesh.visible = visible;
  }

  changeColor(color: Color) {
    this.mesh.material.color = color;
  }
}
