import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";

import {Piece} from "../piece";

export class ResultVotePiece extends Piece {

  constructor() {
    super("ResultVotePiece");
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const material = new MeshStandardMaterial({color: 'grey'});

    const mesh = new Mesh<BufferGeometry, MeshStandardMaterial>(geometry, material);
    mesh.visible = false;
    mesh.name = this.name;


    this.debugFolder?.add(mesh.position, 'x')
      .step(0.01);

    this.debugFolder?.add(mesh.position, 'y')
      .step(0.01);
    this.debugFolder?.add(mesh.position, 'z')
      .step(0.01);

    return mesh;
  }
}
