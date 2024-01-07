import {Piece} from "../piece";


export abstract class VotePiece extends Piece {

  protected constructor(name: string) {
    super(name);
  }

  setVisible(): void {
    this.mesh.visible = true;
    this.mesh.visible = true;
  }
}
