import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";
import {Piece} from "../piece";

export class MissionSuccessPiece extends Piece {

  constructor() {
    super("MissionSuccessPiece");
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const material = new MeshStandardMaterial({color: '#32CD32'})

    const mesh = new Mesh(geometry, material);
    mesh.visible = false;
    return mesh;
  }
}
