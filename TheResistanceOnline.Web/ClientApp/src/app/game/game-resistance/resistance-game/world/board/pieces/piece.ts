import {BufferGeometry, Mesh, MeshStandardMaterial, Scene, Vector3} from "three";
import {ResistanceGame} from "../../../resistance-game";

export abstract class Piece {
  readonly name: string;
  readonly mesh: Mesh<BufferGeometry, MeshStandardMaterial>;
  private readonly scene: Scene;

  protected constructor(name: string) {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;

    this.name = name;
    this.mesh = this.createMesh();
    this.scene.add(this.mesh);
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
