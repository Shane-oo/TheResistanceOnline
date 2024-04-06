import {Group, Material, Scene} from "three";
import {ResistanceGame} from "../resistance-game";
import {Resources} from "../utils/resources";
import {Dispose} from "../utils/dispose";

export class Table {
  // Utils
  private readonly resources: Resources;

  // Scene
  private readonly scene: Scene;
  private readonly model: Group;

  constructor() {
    const resistanceGame = new ResistanceGame();
    this.resources = resistanceGame.resources;
    this.scene = resistanceGame.scene;

    this.model = this.resources.getModelResourceByName('cyberPunkTable').gltf.scene;

    this.scene.add(this.model);
  }

  destroy() {
    this.scene.remove(this.model);
  }
}
