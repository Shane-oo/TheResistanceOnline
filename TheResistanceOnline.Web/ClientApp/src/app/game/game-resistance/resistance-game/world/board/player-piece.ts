import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial, Scene} from "three";
import GUI from "lil-gui";
import {ResistanceGame} from "../../resistance-game";
import {Resources} from "../../utils/resources";

// there will be multiple player pieces, one for each player
export class PlayerPiece {
  private readonly scene: Scene;
  private readonly _playerPiece: Mesh<BufferGeometry, MeshStandardMaterial>;
  private readonly _votePieces: {
    yesVote: Mesh<BufferGeometry, MeshStandardMaterial>,
    noVote: Mesh<BufferGeometry, MeshStandardMaterial>
  };
  private readonly _name: string;
  private readonly position: { x: number, z: number, };
  // Utils
  private readonly resources: Resources;
  // Debug
  private readonly debugFolder?: GUI;

  constructor(name: string, position: { x: number, z: number }) {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;

    this._name = name;
    this.position = position;
    // Player Piece
    this._playerPiece = this.createPlayerPiece();
    this.scene.add(this._playerPiece);
    // Vote Pieces
    this._votePieces = this.createVotePieces();
    this.scene.add(this._votePieces.yesVote);
    this.scene.add(this._votePieces.noVote);

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder(`player-piece-${name}`);
      this.configureDebug();
    }
  }

  get voteYesPiece(): Mesh<BufferGeometry, MeshStandardMaterial> {
    return this._votePieces.yesVote;
  }

  get voteNoPiece(): Mesh<BufferGeometry, MeshStandardMaterial> {
    return this._votePieces.noVote;
  }

  get name(): string {
    return this._name;
  }

  get playerPiece(): Mesh<BufferGeometry, MeshStandardMaterial> {
    return this._playerPiece;
  }

  destroy() {

  }

  private createVotePieces(): {
    yesVote: Mesh<BufferGeometry, MeshStandardMaterial>,
    noVote: Mesh<BufferGeometry, MeshStandardMaterial>
  } {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);

    const yesVoteMesh = new Mesh<BufferGeometry, MeshStandardMaterial>(geometry,
      new MeshStandardMaterial({color: 'green'})
    );
    yesVoteMesh.position.setX(this.position.x - 0.1);
    yesVoteMesh.position.setZ(this.position.z + 0.1);


    const noVoteMesh = new Mesh<BufferGeometry, MeshStandardMaterial>(geometry,
      new MeshStandardMaterial({color: 'red'})
    );
    noVoteMesh.position.setX(this.position.x + 0.1);
    noVoteMesh.position.setZ(this.position.z + 0.1);

    yesVoteMesh.visible = false;
    noVoteMesh.visible = false;

    return {yesVote: yesVoteMesh, noVote: noVoteMesh};
  }

  private createPlayerPiece(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const mesh = new Mesh(geometry, new MeshStandardMaterial({color: 'purple'}));
    mesh.name = this._name;
    mesh.position.setX(this.position.x);
    mesh.position.setZ(this.position.z - 0.05);
    mesh.updateMatrixWorld();

    return mesh;
  }

  private configureDebug() {
    // this.debugFolder?.add(this._voteYesPiece.position, 'x')
    //   .name('PositionX')
    //   .min(-10)
    //   .max(10)
    //   .step(0.1);
    // this.debugFolder?.add(this._voteYesPiece.position, 'y')
    //   .name('PositionY')
    //   .min(-10)
    //   .max(10)
    //   .step(0.1);
    // this.debugFolder?.add(this._voteYesPiece.position, 'z')
    //   .name('PositionZ')
    //   .min(-10)
    //   .max(10)
    //   .step(0.1);
  }

}
