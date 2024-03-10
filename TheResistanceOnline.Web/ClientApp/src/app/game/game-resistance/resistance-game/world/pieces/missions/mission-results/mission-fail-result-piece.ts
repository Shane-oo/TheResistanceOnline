import {BufferGeometry, Mesh, MeshStandardMaterial} from "three";

import {Piece} from "../../piece";
import {TextGeometry} from "three/examples/jsm/geometries/TextGeometry";
import {degToRad} from "three/src/math/MathUtils";

export class MissionFailResultPiece extends Piece {

  constructor() {
    super("MissionFailResultPiece");
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    const fontResource = this.resources.getFontByName('ethnocentricFont');
    const geometry = new TextGeometry("Failure",
      {
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
    geometry.rotateX(degToRad(-90));


    const material = new MeshStandardMaterial({color: 'red'});

    const mesh = new Mesh(geometry, material);
    mesh.visible = false;
    return mesh;
  }
}
