import {AxesHelper, Color, Mesh, Scene, Vector3} from "three";
import {Environment} from "./environment/environment";
import {Resources} from "../utils/resources";
import {Debug} from "../utils/debug";
import {ResistanceGame} from "../resistance-game";
import {Floor} from "./floor";
import {PlayerPiece} from "./pieces/player-piece";
import {MissionRoundPiece} from "./pieces/rounds/mission-round-piece";
import {getMissionTeamCount} from "../utils/helpers";
import {MissionLeaderPiece} from "./pieces/mission-leader-piece";
import {MissionTeamPiece} from "./pieces/mission-team-piece";
import {ResistanceGameRaycasting} from "../resistance-game-raycasting";

export class World {
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

  private readonly missionRoundPositions: { x: number, z: number }[] = [
    {x: -1.5, z: 0},
    {x: -0.75, z: 0},
    {x: 0, z: 0},
    {x: 0.75, z: 0},
    {x: 1.5, z: 0}
  ]

  private readonly scene: Scene;
  private readonly environment: Environment;
  private missionRoundsPieces?: MissionRoundPiece[];
  private readonly missionLeaderPiece: MissionLeaderPiece;
  private missionTeamPieces: { playerPiece: PlayerPiece, missionTeamPiece: MissionTeamPiece }[] = [];

  private readonly floor: Floor;
  private readonly axesHelper: AxesHelper;
  // Utils
  private readonly resources: Resources;
  private readonly debug: Debug;
  private readonly rayCasting: ResistanceGameRaycasting;


  constructor() {
    const resistanceGame = new ResistanceGame()

    this.scene = resistanceGame.scene;

    this.rayCasting = resistanceGame.rayCasting;
    this.resources = resistanceGame.resources;
    this.debug = resistanceGame.debug;

    // Add all children of World
    this.floor = new Floor();
    this.missionLeaderPiece = new MissionLeaderPiece();
    this.axesHelper = new AxesHelper(5);
    this.scene.add(this.axesHelper);

    this.environment = new Environment();
  }

  private _playerPieces?: PlayerPiece[];

  get playerPieces(): PlayerPiece[] | undefined {
    return this._playerPieces;
  }

  update() {
  }

  destroy() {
    this.environment.destroy();
    this.rayCasting.destroy();
    this.floor.destroy();
  }

  createPlayerPieces(players: string[]) {
    const playerPieces: PlayerPiece[] = [];
    for (let i = 0; i < players.length; i++) {
      const piece = new PlayerPiece(players[i], this.playerPositions[i]);
      playerPieces.push(piece);
    }
    this._playerPieces = playerPieces;

    this.createMissionRoundPieces();
  }

  setMissionLeader(player: string) {
    const playerPiece = this.getPlayerPieceByName(player);
    if (playerPiece) {
      playerPiece.isMissionLeader = true;
      const position = playerPiece.mesh.position.clone();
      position.setZ(position.z - 0.15); // above the player piece

      this.missionLeaderPiece.movePiece(position);

    }
  }

  addMissionTeamMember(player: string) {
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

  removeMissionTeamMember(player: string) {
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

  movePlayersToMiddle(missionMembers: string[]) {
    const position = new Vector3(0, 0, -0.75);
    for (let i = 0; i < missionMembers.length; i++) {
      const playerPiece = this.getPlayerPieceByName(missionMembers[i]);
      if (playerPiece) {
        const xPosition = (i - 1) / 2;
        position.setX(xPosition);
        playerPiece.movePiece(position);

        // move mission leader piece with
        if (playerPiece.isMissionLeader) {
          position.setZ(position.z - 0.15); // above the player piece
          this.missionLeaderPiece.movePiece(position);
          position.setZ(position.z + 0.15); // undo z change
        }
      }
    }
  }

  showVotingPieces(playerName: string) {
    const voteMeshes: Mesh[] = [];
    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      playerPiece.showVotingPieces();
      voteMeshes.push(playerPiece.votePieces.approveVotePiece.mesh);
      voteMeshes.push(playerPiece.votePieces.rejectVotePiece.mesh);
      this.rayCasting.selectableObjects = voteMeshes;
    }
  }

  hideVotingPieces(playerName: string) {
    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      playerPiece.hideVotingPieces();
    }
  }

  showVoteResultPieces(playerName: string) {
    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      playerPiece.showVoteResultPieces();
    }
  }

  changeVoteResultPiecesColor(playerName: string, color: Color) {
    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      const votePiece = playerPiece.votePieces.resultVotePiece;
      votePiece.changeColor(color);
    }
  }

  removeVoteResults() {
    for (const playerPiece of this._playerPieces!) {
      playerPiece.hideVoteResultPieces();
    }
  }

  private createMissionRoundPieces() {
    const rounds = [];

    for (let i = 1; i < 6; i++) {
      rounds.push(new MissionRoundPiece(i, getMissionTeamCount(i, this._playerPieces!.length), this.missionRoundPositions[i - 1]));
    }

    this.missionRoundsPieces = rounds;
  }

  private getPlayerPieceByName(name: string) {
    return this._playerPieces?.find(p => p.name === name);
  }
}
