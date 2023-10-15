import {ResistanceGame} from "../../resistance-game";
import {Group, Mesh, Scene, Vector3} from "three";
import {ModelResource, Resources} from "../../utils/resources";
import GUI from "lil-gui";

export class MissionLeaderPiece {
  private readonly scene: Scene;
  private readonly modelResource: ModelResource;
  private readonly model: Group;

  //Utils
  private readonly resources: Resources;

  //Debug
  private readonly debugFolder?: GUI;

  constructor() {
    const resistanceGame = new ResistanceGame();

    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;

    // Board
    this.modelResource = this.resources.getModelResourceByName('missionLeaderPiece');
    this.model = this.modelResource.gltf.scene;
    this.configureModel();
    this.scene.add(this.model);


    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder('missionLeaderPiece');

      this.configureDebug();
    }
  }

  private configureDebug() {
    this.debugFolder?.add(this.model.scale, 'x')
      .name('scaleX')
      .min(0)
      .max(2)
      .step(0.001)
    this.debugFolder?.add(this.model.scale, 'y')
      .name('ScaleY')
      .min(-10)
      .max(10)
      .step(0.1);
    this.debugFolder?.add(this.model.scale, 'z')
      .name('ScaleZ')
      .min(-10)
      .max(10)
      .step(0.1);
  }

  movePiece(position: Vector3) {
    this.model.position.setX(position.x);
    this.model.position.setZ(position.z);
  }

  private configureModel() {

    this.model.scale.set(0.075, 0.15, 0.15);
    this.model.position.setY(0.05);
    this.model.traverse((child) => {
      if (child instanceof Mesh) {
        child.castShadow = true;
        child.receiveShadow = true;
      }
    });
  }
}
