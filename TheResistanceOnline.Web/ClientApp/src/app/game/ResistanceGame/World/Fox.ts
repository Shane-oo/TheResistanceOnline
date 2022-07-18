import ResistanceGame from '../ResistanceGame';
import * as THREE from 'three';
import Resources from '../Utils/Resources';
import { GLTF } from 'three/examples/jsm/loaders/GLTFLoader';
import Time from '../Utils/Time';


export default class Fox {
  public resistanceGame: ResistanceGame;
  public scene: THREE.Scene;
  public resources: Resources;
  public model!: THREE.Group;
  public resource!: GLTF;
  public animation!: THREE.AnimationMixer;
  public animationAction!: THREE.AnimationAction;
  public time:Time;
  constructor() {
    this.resistanceGame = new ResistanceGame();
    this.scene = this.resistanceGame.scene;
    this.resources = this.resistanceGame.resources;
    this.time = this.resistanceGame.time;
    let foxModel = this.resources.gltfModels.find(obj => {
      return obj.name === 'foxModel';
    });
    if(foxModel) {
      this.resource = foxModel.model;
    }

    this.setModel();
    this.setAnimation();
  }

  private setModel() {
    this.model = this.resource.scene;
    this.model.scale.set(0.02, 0.02, 0.02);
    this.scene.add(this.model);

    this.model.traverse((child) => {
      if(child instanceof THREE.Mesh) {
        child.castShadow = true;
      }
    });
  }

  private setAnimation() {
    this.animation = new THREE.AnimationMixer(this.model);
    this.animationAction = this.animation.clipAction(this.resource.animations[0]);
    this.animationAction.play();
  }

  public update(){
    this.animation.update(this.time.delta * 0.001);
  }
}
