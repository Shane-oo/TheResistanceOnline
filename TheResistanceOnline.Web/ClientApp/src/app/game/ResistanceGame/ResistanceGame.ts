import Sizes from './Utils/Sizes';
import Time from './Utils/Time';
import Resources from './Utils/Resources';
import Camera from './Camera';
import * as THREE from 'three';
import Renderer from './Renderer';
import World from './World/World';
import sources from './sources';
import Debug from './Utils/Debug';

export default class ResistanceGame {
  public debug!: Debug;
  public canvas!: HTMLCanvasElement;
  public sizes!: Sizes;
  public time!: Time;
  public scene!: THREE.Scene;
  public camera!: Camera;
  public renderer!: Renderer;
  public world!: World;
  public resources!: Resources;

  private static instance: ResistanceGame;

  constructor(canvas?: HTMLCanvasElement) {
    if(ResistanceGame.instance) {
      return ResistanceGame.instance;
    }
    ResistanceGame.instance = this;

    // Options
    if(canvas) {this.canvas = canvas;}

    // Setup
    this.debug = new Debug();
    this.sizes = new Sizes();
    this.time = new Time();
    this.scene = new THREE.Scene();
    this.resources = new Resources([sources]);
    this.camera = new Camera();
    this.renderer = new Renderer();
    this.world = new World();
    // Sizes resize event
    this.sizes.on('resize', () => {
      this.resize();
    });
    // Time tick event
    this.time.on('tick', () => {
      this.update();
    });

    console.log(this);
  }


  private resize() {
    this.camera.resize();
    this.renderer.resize();
  }

  private update() {
    this.camera.update();
    this.world.update();
    this.renderer.update();

  }

  /*
    If have more complex project with a lot to destroy, you
    may want to create a destroy() method for each class
  */
  public destroy() {
    this.sizes.off('resize');
    this.time.off('tick');

    // Traverse the whole scene
    this.scene.traverse((child) => {
      if(child instanceof THREE.Mesh) {
        child.geometry.dispose();

        for(const key in child.material) {
          const value = child.material[key];

          // not the most efficient way of disposing
          if(value && typeof value.dispose === 'function') {
            value.dispose();
          }
        }
      }
    });

    this.camera.controls.dispose();
    this.renderer.instance.dispose();
    if(this.debug.active) {
      this.debug.gui.destroy();
    }
  }
}
