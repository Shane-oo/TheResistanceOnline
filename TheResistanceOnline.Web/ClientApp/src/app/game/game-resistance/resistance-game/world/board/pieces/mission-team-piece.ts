import {Piece} from "./piece";
import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";

export class MissionTeamPiece extends Piece {


  constructor() {
    super("MissionTeamPiece");

  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const mesh = new Mesh(geometry, new MeshStandardMaterial({color: 'yellow'}));
    mesh.name = this.name;

    return mesh;
  }


}
