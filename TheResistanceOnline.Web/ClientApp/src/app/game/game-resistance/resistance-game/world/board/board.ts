import {Scene, Vector3} from "three";
import {Resources} from "../../utils/resources";
import GUI from "lil-gui";
import {PlayerPiece} from "./pieces/player-piece";
import {ResistanceGame} from "../../resistance-game";
import {MissionLeaderPiece} from "./pieces/mission-leader-piece";
import {MissionTeamPiece} from "./pieces/mission-team-piece";

export class Board {
  private readonly playerPositions: { x: number, z: number }[] = [
    {x: -2, z: -1.25}, // top left
    {x: 0, z: -1.25}, // top middle
    {x: 2, z: -1.25}, // top right

    {x: 2.75, z: 0.5}, // right top
    {x: 2.75, z: -0.5}, // right bottom

    {x: 2, z: 1.25}, // bottom right
    {x: 0, z: 1.25}, // bottom middle
    {x: -2, z: 1.25}, // bottom left

    {x: -2.75, z: 0.5}, // left bottom
    {x: -2.75, z: -0.5} // left top
  ];

  private readonly scene: Scene;
  private readonly missionLeaderPiece: MissionLeaderPiece;
  private missionTeamPieces: { playerPiece: PlayerPiece, missionTeamPiece: MissionTeamPiece }[] = [];
  // Utils
  private readonly resources: Resources;
  // Debug
  private readonly debugFolder?: GUI;

  constructor() {
    const resistanceGame = new ResistanceGame();

    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;


    // Mission Leader
    this.missionLeaderPiece = new MissionLeaderPiece();

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder('board');

      this.configureDebug();
    }
  }

  private _playerPieces?: PlayerPiece[];

  get playerPieces(): PlayerPiece[] | undefined {
    return this._playerPieces;
  }

  createPlayerPieces(players: string[]) {
    const playerPieces: PlayerPiece[] = [];
    const position = new Vector3();
    for (let i = 0; i < players.length; i++) {
      const piece = new PlayerPiece(players[i], this.playerPositions[i]);

      position.set(this.playerPositions[i].x, 0, this.playerPositions[i].z)
      piece.movePiece(position);

      playerPieces.push(piece);
    }

    this._playerPieces = playerPieces;
  }

  moveLeaderPiece(player: string) {
    const playerPiece = this.getPlayerPieceByName(player);
    if (playerPiece) {
      const position = playerPiece.mesh.position.clone();
      position.setZ(position.z - 0.15); // above the player piece

      this.missionLeaderPiece.movePiece(position);
    }
  }

  addMissionTeamMemberPieceToPlayer(player: string) {
    const playerPiece = this.getPlayerPieceByName(player);
    if (playerPiece) {
      const missionTeamPiece = new MissionTeamPiece();

      const position = playerPiece.mesh.position.clone();
      position.setZ(position.z + 0.15); // below the player piece
      missionTeamPiece.movePiece(position);

      this.missionTeamPieces.push({
        playerPiece: playerPiece,
        missionTeamPiece: missionTeamPiece
      });
    }
  }

  removeMissionTeamMemberPieceFromPlayer(player: string) {
    const playerPiece = this.getPlayerPieceByName(player);
    if (playerPiece) {
      const missionTeamPiece = this.missionTeamPieces.find(p => p.playerPiece === playerPiece);
      if (missionTeamPiece) {
        this.scene.remove(missionTeamPiece.missionTeamPiece.mesh);
        missionTeamPiece?.missionTeamPiece.destroy();

        this.missionTeamPieces = this.missionTeamPieces.filter(p => p !== missionTeamPiece);
      }
    }
  }

  destroy() {
    if (this._playerPieces) {
      for (const piece of this._playerPieces) {
        piece.destroy();
      }
    }
    this.missionLeaderPiece.destroy();
  }

  private getPlayerPieceByName(name: string) {
    return this._playerPieces?.find(p => p.name === name);
  }

  private configureDebug() {
    if (this.debugFolder) {

    }
  }

}
