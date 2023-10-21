import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial, Vector3} from "three";
import {Piece} from "./piece";
import {ApproveVotePiece} from "./voting/approve-vote-piece";
import {RejectVotePiece} from "./voting/reject-vote-piece";

// there will be multiple player pieces, one for each player
export class PlayerPiece extends Piece {
  readonly votePieces: {
    approveVotePiece: ApproveVotePiece,
    rejectVotePiece: RejectVotePiece
  };
  private readonly position: { x: number, z: number };

  constructor(name: string, position: { x: number, z: number }) {
    super(name);

    this.position = position;
    // Vote Pieces
    this.votePieces = this.createVotePieces();

  }


  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const mesh = new Mesh(geometry, new MeshStandardMaterial({color: 'purple'}));
    mesh.name = this.name;

    mesh.updateMatrixWorld();

    return mesh;
  }

  private createVotePieces(): {
    approveVotePiece: ApproveVotePiece,
    rejectVotePiece: RejectVotePiece
  } {

    const approveVotePiece = new ApproveVotePiece(
      this.name + "-ApproveVotePiece");
    approveVotePiece.movePiece(new Vector3(this.position.x - 0.1, 0, this.position.z + 0.1));

    const rejectVotePiece = new RejectVotePiece(
      this.name + "-RejectVotePiece"
    );
    rejectVotePiece.movePiece(new Vector3(this.position.x + 0.1, 0, this.position.z + 0.1));

    return {approveVotePiece: approveVotePiece, rejectVotePiece: rejectVotePiece};
  }


  destroy() {
    super.destroy();
    this.votePieces.approveVotePiece.destroy();
    this.votePieces.rejectVotePiece.destroy();
  }

}
