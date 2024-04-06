import {Piece} from "./piece";
import {BufferGeometry, Mesh, MeshStandardMaterial} from "three";

export class MissionTeamPiece extends Piece {


  constructor() {
    super("MissionTeamPiece");
  }

  createMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {

    const soldiers = this.resources.getModelResourceByName('soldiers').gltf;

    // Soldier0, Soldier1, Soldier2
    const soldierName = "Soldier" + Math.floor(Math.random() * 3);
    const randomSoldier = soldiers.scene.children.find(c => c.name === soldierName)?.clone(true) as Mesh<BufferGeometry, MeshStandardMaterial>;

    randomSoldier.name = this.name;

    console.log("Random soldier ", randomSoldier)

    return randomSoldier;
  }


}
