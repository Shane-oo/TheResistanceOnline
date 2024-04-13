import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial, Vector3} from "three";

import {Piece} from "./piece";
import {ApproveVotePiece} from "./voting/approve-vote-piece";
import {RejectVotePiece} from "./voting/reject-vote-piece";
import {ResultVotePiece} from "./voting/result-vote-piece";
import {MissionSuccessPiece} from "./missions/mission-choices/mission-success-piece";
import {MissionSabotagePiece} from "./missions/mission-choices/mission-sabotage-piece";
import {Position} from "../../resistance-game-models";

// there will be multiple player pieces, one for each player
export class PlayerPiece extends Piece {
  public readonly position: Position;
  private readonly _votePieces: {
    approveVotePiece: ApproveVotePiece,
    rejectVotePiece: RejectVotePiece,
    resultVotePiece: ResultVotePiece
  };
  private readonly _missionChoicePieces: {
    missionSuccessPiece: MissionSuccessPiece,
    missionSabotagePiece: MissionSabotagePiece
  };

  constructor(name: string, position: Position) {
    super(name);

    // Move Piece
    this.position = position;
    const positionVector = new Vector3(this.position.x, this.position.y, this.position.z)
    this.movePiece(positionVector);

    // Vote Pieces
    this._votePieces = this.createVotePieces();
    // Mission Choice Pieces
    this._missionChoicePieces = this.createMissionChoicePieces();
  }

  get votePieces(): {
    approveVotePiece: ApproveVotePiece;
    rejectVotePiece: RejectVotePiece,
    resultVotePiece: ResultVotePiece
  } {
    return this._votePieces;
  }

  get missionChoicePieces(): {
    missionSuccessPiece: MissionSuccessPiece,
    missionSabotagePiece: MissionSabotagePiece
  } {
    return this._missionChoicePieces;
  }

  private _isMissionLeader: boolean = false;

  get isMissionLeader(): boolean {
    return this._isMissionLeader;
  }

  set isMissionLeader(leader: boolean) {
    this._isMissionLeader = leader;
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const mesh = new Mesh(geometry, new MeshStandardMaterial({color: 'purple'}));
    mesh.material.envMapIntensity = 1;
    mesh.material.roughness = 0.2;
    mesh.material.metalness = 1.0;
    mesh.name = this.name;

    return mesh;
  }

  destroy() {
    super.destroy();
    this._votePieces.approveVotePiece.destroy();
    this._votePieces.rejectVotePiece.destroy();
  }

  showVotingPieces() {
    this._votePieces.approveVotePiece.setVisible(true);
    this._votePieces.rejectVotePiece.setVisible(true);
  }

  hideVotingPieces() {
    this._votePieces.approveVotePiece.setVisible(false);
    this._votePieces.rejectVotePiece.setVisible(false);
  }

  showMissionSuccessPiece() {
    this._missionChoicePieces.missionSuccessPiece.setVisible(true);
  }

  showMissionSabotagePiece() {
    this._missionChoicePieces.missionSabotagePiece.setVisible(true);
  }

  hideMissionChoicePieces() {
    this._missionChoicePieces.missionSabotagePiece.setVisible(false);
    this._missionChoicePieces.missionSuccessPiece.setVisible(false);
  }


  showVoteResultPieces() {
    this._votePieces.resultVotePiece.setVisible(true);
  }

  hideVoteResultPieces() {
    this._votePieces.resultVotePiece.setVisible(false);
    this._votePieces.resultVotePiece.changeColor(ResultVotePiece.defaultColor);
  }


  // hmm i am creating unnecessary pieces for everyone on the table when really only need to create once for the client player
  private createVotePieces(): {
    approveVotePiece: ApproveVotePiece,
    rejectVotePiece: RejectVotePiece,
    resultVotePiece: ResultVotePiece
  } {

    const approveVotePiece = new ApproveVotePiece();
    approveVotePiece.movePiece(new Vector3(this.position.x - 0.1, this.position.y, this.position.z - 0.3));

    const rejectVotePiece = new RejectVotePiece();
    rejectVotePiece.movePiece(new Vector3(this.position.x + 0.1, this.position.y, this.position.z - 0.3));

    const resultVotePiece = new ResultVotePiece();
    resultVotePiece.movePiece(new Vector3(this.position.x, this.position.y, this.position.z + 0.3));

    return {approveVotePiece: approveVotePiece, rejectVotePiece: rejectVotePiece, resultVotePiece: resultVotePiece};
  }

  private createMissionChoicePieces(): {
    missionSuccessPiece: MissionSuccessPiece,
    missionSabotagePiece: MissionSabotagePiece
  } {

    const missionSuccessPiece = new MissionSuccessPiece();
    missionSuccessPiece.movePiece(new Vector3(this.position.x - 0.1, this.position.y, this.position.z - 0.3));

    const missionSabotagePiece = new MissionSabotagePiece();
    missionSabotagePiece.movePiece(new Vector3(this.position.x + 0.1, this.position.y, this.position.z - 0.3));

    return {missionSuccessPiece: missionSuccessPiece, missionSabotagePiece: missionSabotagePiece};
  }

}
