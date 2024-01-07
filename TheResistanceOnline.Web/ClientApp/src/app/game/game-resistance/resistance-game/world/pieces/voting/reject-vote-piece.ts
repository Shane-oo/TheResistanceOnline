import {VotePiece} from "./vote-piece";
import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";

export class RejectVotePiece extends VotePiece {

  constructor(name: string) {
    super(name);
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
