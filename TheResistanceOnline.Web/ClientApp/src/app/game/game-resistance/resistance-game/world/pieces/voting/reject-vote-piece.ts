import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";
import {Piece} from "../piece";

export class RejectVotePiece extends Piece {

  constructor() {
    super("RejectVotePiece");
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const material = new MeshStandardMaterial({color: 'red'});

    const mesh = new Mesh<BufferGeometry, MeshStandardMaterial>(geometry, material);
    mesh.visible = false;
    mesh.name = this.name;
    return mesh;
  }
}
