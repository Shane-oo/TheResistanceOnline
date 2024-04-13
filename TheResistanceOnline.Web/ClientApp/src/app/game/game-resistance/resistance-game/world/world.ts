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
import {MissionResultsModel, PositionOnBoard, Team} from "../../game-resistance.models";
import {Piece} from "./pieces/piece";
import {MissionSuccessResultPiece} from "./pieces/missions/mission-results/mission-success-result-piece";
import {MissionFailResultPiece} from "./pieces/missions/mission-results/mission-fail-result-piece";
import {MissionSabotagePiece} from "./pieces/missions/mission-choices/mission-sabotage-piece";
import {MissionSuccessPiece} from "./pieces/missions/mission-choices/mission-success-piece";
import {Table} from "./table";
import {Position} from "../resistance-game-models";

export class World {
  private readonly playerPositions: Position[] = [
    {x: -0.8, y: .5, z: -0.5}, // top left
    {x: 0, y: .5, z: -0.5}, // top middle
    {x: .8, y: .5, z: -0.5}, // top right

    {x: 1.075, y: .5, z: -0.15}, // right bottom
    {x: 1.075, y: .5, z: 0.35}, // right top

    {x: .8, y: .5, z: 0.725}, // bottom right
    {x: 0, y: .5, z: 0.725}, // bottom middle
    {x: -.8, y: .5, z: 0.725}, // bottom left

    {x: -1.175, y: .5, z: -0.15}, // left top
    {x: -1.175, y: .5, z: 0.35} // left bottom
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
  private missionResultPieces: Piece[] = [];
  private missionTeamPieces: { playerPiece: PlayerPiece, missionTeamPiece: MissionTeamPiece }[] = [];

  private readonly floor: Floor;
  private readonly table: Table;
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
    this.table = new Table();
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

  private getPositionOnBoard(x: number, z: number): PositionOnBoard {
    const playerPositionIndex = this.playerPositions.findIndex(p => p.x === x && p.z === z);
    if (playerPositionIndex <= 2) {
      return PositionOnBoard.Top;
    } else if (playerPositionIndex > 2 && playerPositionIndex <= 4) {
      return PositionOnBoard.Right;
    } else if (playerPositionIndex > 4 && playerPositionIndex <= 7) {
      return PositionOnBoard.Bottom;
    } else {
      return PositionOnBoard.Left;
    }
  }

  addMissionTeamMember(player: string) {
    const playerPiece = this.getPlayerPieceByName(player);
    if (playerPiece) {
      const missionTeamPiece = new MissionTeamPiece();

      const position = new Vector3(playerPiece.position.x, playerPiece.position.y, playerPiece.position.z);

      // bit of a hack
      const positionOnBoard = this.getPositionOnBoard(position.x, position.z);
      if (positionOnBoard === PositionOnBoard.Top) {
        // top of board move z down
        position.setZ(position.z + .15);
      } else if (positionOnBoard === PositionOnBoard.Right) {
        // right of board move x left
        position.setX(position.x - .15);
      } else if (positionOnBoard === PositionOnBoard.Bottom) {
        //bottom of board move z up
        position.setZ(position.z - .15);
      } else {
        // left of board move x right
        position.setX(position.x + .15);
      }

      missionTeamPiece.movePiece(position);
      missionTeamPiece.lookAt(new Vector3(0, .5, 0));

      this.missionTeamPieces.push({
        playerPiece: playerPiece,
        missionTeamPiece: missionTeamPiece
      });
    }
  }

  removeMissionTeamMember(player: string) {
    const playerPiece = this.getPlayerPieceByName(player);
    if (playerPiece) {
      this.removePlayersMissionTeamPiece(playerPiece);
    }
  }

  clearMissionTeamPieces() {
    for (const playerPiece of this._playerPieces ?? []) {
      this.removePlayersMissionTeamPiece(playerPiece);
    }
  }

  clearMissionResults() {
    for (const missionResultPiece of this.missionResultPieces) {
      this.scene.remove(missionResultPiece.mesh);
      missionResultPiece.destroy();
    }
    this.missionResultPieces = [];
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

  showMissionChoicePieces(showSuccessAndFail: boolean, playerName: string) {
    const missionChoiceMeshes: Mesh[] = [];

    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      playerPiece.showMissionSuccessPiece();
      missionChoiceMeshes.push(playerPiece.missionChoicePieces.missionSuccessPiece.mesh);

      if (showSuccessAndFail) {
        playerPiece.showMissionSabotagePiece();
        missionChoiceMeshes.push(playerPiece.missionChoicePieces.missionSabotagePiece.mesh);
      }

      this.rayCasting.selectableObjects = missionChoiceMeshes;
    }
  }

  hideMissionChoices(playerName: string) {
    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      playerPiece.hideMissionChoicePieces();
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

  showMissionResults(missionResults: MissionResultsModel) {
    let resultPiece: Piece = missionResults.missionSuccessful ? new MissionSuccessResultPiece() : new MissionFailResultPiece();
    resultPiece.setVisible(true);
    resultPiece.movePiece(new Vector3(-0.75, 0, -0.75));

    this.missionResultPieces.push(resultPiece);

    const missionChoicePosition = new Vector3(-0.25, 0, -0.5);

    for (let i = 0; i < missionResults.sabotageChoices; i++) {
      const sabotagePiece = new MissionSabotagePiece();
      sabotagePiece.setVisible(true);
      sabotagePiece.movePiece(missionChoicePosition.clone());
      missionChoicePosition.x += 0.25;

      this.missionResultPieces.push(sabotagePiece);

    }

    for (let i = 0; i < missionResults.successChoices; i++) {
      const successPiece = new MissionSuccessPiece();
      successPiece.setVisible(true);
      successPiece.movePiece(missionChoicePosition.clone());
      missionChoicePosition.x += 0.25;

      this.missionResultPieces.push(successPiece);
    }

  }

  showPlayerTeams(playerNameToTeam: Map<string, Team>) {
    for (let [playerName, team] of playerNameToTeam) {
      const color = new Color(team === Team.Resistance ? 'blue' : 'red');

      const playerPiece = this.getPlayerPieceByName(playerName);
      if (playerPiece) {
        playerPiece.changeColor(color);
      }
    }
  }

  showWinners(winners: Team) {
    // can't be bothered right now
    console.log(`${winners === Team.Resistance ? 'Resistance' : 'Spies'} won!`);
  }

  public getPlayerPieceByName(name: string) {
    return this._playerPieces?.find(p => p.name === name);
  }

  private removePlayersMissionTeamPiece(playerPiece: PlayerPiece) {
    const missionTeamPiece = this.missionTeamPieces.find(p => p.playerPiece === playerPiece);
    if (missionTeamPiece) {
      this.scene.remove(missionTeamPiece.missionTeamPiece.mesh);
      missionTeamPiece?.missionTeamPiece.destroy();

      this.missionTeamPieces = this.missionTeamPieces.filter(p => p !== missionTeamPiece);
    }
  }

  private createMissionRoundPieces() {
    const rounds = [];

    for (let i = 1; i < 6; i++) {
      rounds.push(new MissionRoundPiece(i, getMissionTeamCount(i, this._playerPieces!.length), this.missionRoundPositions[i - 1]));
    }

    this.missionRoundsPieces = rounds;
  }

  getCameraStartingPosition(playerName: string): Position {
    let position: Position = {x: 0, y: 0, z: 0};
    const playerPiece = this.getPlayerPieceByName(playerName);
    if (playerPiece) {
      position.x = playerPiece.position.x;
      position.z = playerPiece.position.z;

      // bit of a hack
      const positionOnBoard = this.getPositionOnBoard(playerPiece.position.x, playerPiece.position.z);
      if (positionOnBoard === PositionOnBoard.Top) {
        // top of board move z up
        position.z -= 1.25;
      } else if (positionOnBoard === PositionOnBoard.Right) {
        // right of board move x right
        position.x += 1.25;
      } else if (positionOnBoard === PositionOnBoard.Bottom) {
        //bottom of board move z down
        position.z += 1.25;
      } else {
        // left of board move x left
        position.x -= 1.25;
      }
    }
    console.log(position);

    return position;
  }
}
