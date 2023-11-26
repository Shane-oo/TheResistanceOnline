import {VotePiece} from "./vote-piece";
import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial} from "three";

export class ApproveVotePiece extends VotePiece {
  constructor(name: string) {
    super(name);
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const material = new MeshStandardMaterial(
      {
        color: 'green',
        visible: false
      }
    );

    return new Mesh<BufferGeometry, MeshStandardMaterial>(geometry, material);
  }

}
