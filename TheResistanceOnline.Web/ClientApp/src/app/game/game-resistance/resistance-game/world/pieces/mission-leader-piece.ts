import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";
import {Piece} from "./piece";

export class MissionLeaderPiece extends Piece {

  constructor() {
    super("MissionLeaderPiece");
  }


  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const mesh = new Mesh(geometry, new MeshStandardMaterial({color: 'white'}));
    mesh.name = this.name;
    return mesh;
  }

}
