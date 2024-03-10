import {Piece} from "../piece";
import {BoxGeometry, BufferGeometry, Mesh, MeshStandardMaterial, Vector3} from "three";
import {TextGeometry} from "three/examples/jsm/geometries/TextGeometry";
import {degToRad} from "three/src/math/MathUtils";

export class MissionRoundPiece extends Piece {
  private readonly missionNumber: number;
  private readonly missionTeamNumber: number;

  private readonly missionNumberText: Mesh<TextGeometry, MeshStandardMaterial>;
  private readonly missionTeamNumberText: Mesh<TextGeometry, MeshStandardMaterial>;

  private readonly position: { x: number, z: number };

  constructor(missionNumber: number, missionTeamNumber: number, position: { x: number, z: number }) {
    super(`MissionRoundPiece${missionNumber}`);

    // Move Piece
    this.position = position;
    const positionVector = new Vector3(this.position.x, 0, this.position.z)
    this.movePiece(positionVector);

    this.missionNumber = missionNumber;
    this.missionTeamNumber = missionTeamNumber;


    this.missionNumberText = this.createMissionNumber();
    this.missionTeamNumberText = this.createMissionTeamNumber();

    // Debug
    this.configureDebug();
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const geometry = new BoxGeometry(0.125, 0.125, 0.125);
    const mesh = new Mesh(geometry, new MeshStandardMaterial({color: 'blue'}));
    mesh.name = this.name;


    return mesh;
  }

  private createTextGeometry(value: string): TextGeometry {
    const fontResource = this.resources.getFontByName('ethnocentricFont');

    return new TextGeometry(value, {
      font: fontResource.font,
      size: 0.2,
      height: 0.05,
      curveSegments: 20,
      bevelEnabled: true,
      bevelThickness: 0.03,
      bevelSize: 0.02,
      bevelOffset: 0,
      bevelSegments: 4
    });
  }

  private createMissionNumber(): Mesh<TextGeometry, MeshStandardMaterial> {
    const textGeometry = this.createTextGeometry(this.missionNumber.toString());

    const material = new MeshStandardMaterial({color: 'red'});

    const missionText = new Mesh<TextGeometry, MeshStandardMaterial>(textGeometry, material);
    missionText.rotateX(degToRad(-90));
    missionText.position.set(this.position.x - 0.08, 0.25, this.position.z);

    this.scene.add(missionText);

    return missionText;
  }

  private createMissionTeamNumber(): Mesh<TextGeometry, MeshStandardMaterial> {
    const textGeometry = this.createTextGeometry(this.missionTeamNumber.toString());

    const material = new MeshStandardMaterial({color: 'brown'});

    const missionText = new Mesh<TextGeometry, MeshStandardMaterial>(textGeometry, material);
    missionText.rotateX(degToRad(-90));
    missionText.position.set(this.position.x - 0.08, 0.25, this.position.z + 0.5);

    this.scene.add(missionText);

    return missionText;
  }

  private configureDebug() {
    if (this.debugFolder) {

      this.debugFolder.add(this.missionNumberText!.position, 'x')
        .name('X')
        .min(-100)
        .max(100)
        .step(0.01);
      this.debugFolder.add(this.missionNumberText!.position, 'y')
        .name('Y')
        .min(-100)
        .max(100)
        .step(0.01);
      this.debugFolder.add(this.missionNumberText!.position, 'z')
        .name('Z')
        .min(-100)
        .max(100)
        .step(0.01);
      // if (this.debugFolder) {
      //   this.debugFolder.add(this._perspectiveCamera, 'fov')
      //     .name('fov')
      //     .min(0)
      //     .max(100)
      //     .step(0.01)
      //     .onChange(() => this._perspectiveCamera.updateProjectionMatrix());
      //
      //   this.debugFolder.add(this._perspectiveCamera.position, 'x')
      //     .name('cameraX')
      //     .min(-100)
      //     .max(100)
      //     .step(0.01);
      //   this.debugFolder.add(this._perspectiveCamera.position, 'y')
      //     .name('cameraY')
      //     .min(-100)
      //     .max(100)
      //     .step(0.01);
      //   this.debugFolder.add(this._perspectiveCamera.position, 'z')
      //     .name('cameraZ')
      //     .min(-100)
      //     .max(100)
      //     .step(0.01);
      // }
    }
  }
}
