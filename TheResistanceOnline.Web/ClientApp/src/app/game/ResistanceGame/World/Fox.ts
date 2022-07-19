import ResistanceGame from '../ResistanceGame';
import * as THREE from 'three';
import Resources from '../Utils/Resources';
import { GLTF } from 'three/examples/jsm/loaders/GLTFLoader';
import Time from '../Utils/Time';
import Debug from '../Utils/Debug';


export default class Fox {
  public resistanceGame: ResistanceGame;
  public scene: THREE.Scene;
  public resources: Resources;
  public model!: THREE.Group;
  public resource!: GLTF;
  public animation!: THREE.AnimationMixer;
  public animationActionCurrent!: THREE.AnimationAction;
  public animationActionIdle!: THREE.AnimationAction;
  public animationActionWalking!: THREE.AnimationAction;
  public animationActionRunning!: THREE.AnimationAction;

  public time: Time;
  public debug: Debug;

  // TODO what is debug folder type
  public debugFolder: any;

  constructor() {
    this.resistanceGame = new ResistanceGame();
    this.scene = this.resistanceGame.scene;
    this.resources = this.resistanceGame.resources;
    this.time = this.resistanceGame.time;
    this.debug = this.resistanceGame.debug;
    // Debug
    if(this.debug.active) {
      this.debugFolder = this.debug.gui.addFolder('fox');
    }

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

    this.animationActionIdle = this.animation.clipAction(this.resource.animations[0]);
    this.animationActionWalking = this.animation.clipAction(this.resource.animations[1]);
    this.animationActionRunning = this.animation.clipAction(this.resource.animations[2]);

    this.animationActionCurrent = this.animationActionIdle;
    this.animationActionCurrent.play();

    // Debug
    if(this.debug.active) {
      const debugObject = {
        playIdle: () => {
          this.playAnimation(this.animationActionIdle);
        },
        playWalking: () => {
          this.playAnimation(this.animationActionWalking);
        },
        playRunning: () => {
          this.playAnimation(this.animationActionRunning);
        }
      };

      this.debugFolder.add(debugObject, 'playIdle');
      this.debugFolder.add(debugObject, 'playWalking');
      this.debugFolder.add(debugObject, 'playRunning');
    }
  }

  public update() {
    this.animation.update(this.time.delta * 0.001);
  }

  public playAnimation = (animation: THREE.AnimationAction) => {
    const newAction = animation;
    const oldAction = this.animationActionCurrent;

    newAction.reset();
    newAction.play();
    newAction.crossFadeFrom(oldAction, 1, true);
    this.animationActionCurrent = newAction;
  };
}
