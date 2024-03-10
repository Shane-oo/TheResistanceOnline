import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";
import {Piece} from "../../piece";

export class MissionSabotagePiece extends Piece {

  constructor() {
    super("MissionSabotagePiece");
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const material = new MeshStandardMaterial({color: '#FF3131'})

    const mesh = new Mesh(geometry, material);
    mesh.visible = false;
    return mesh;
  }
}
