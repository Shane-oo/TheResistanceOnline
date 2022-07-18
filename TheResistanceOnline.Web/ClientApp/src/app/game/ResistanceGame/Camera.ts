import ResistanceGame from './ResistanceGame';
import Sizes from './Utils/Sizes';
import Time from './Utils/Time';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

export default class Camera {
  public resistanceGame: ResistanceGame;
  public canvas: HTMLCanvasElement ;
  public sizes: Sizes ;
  public scene: THREE.Scene ;
  public instance: THREE.PerspectiveCamera =new THREE.PerspectiveCamera();
  public controls!: OrbitControls;

  constructor() {
    this.resistanceGame = new ResistanceGame();
    this.sizes = this.resistanceGame.sizes;
    this.scene = this.resistanceGame.scene;
    this.canvas = this.resistanceGame.canvas;
    this.setInstance();
    this.setOrbitControls();

  }

  private setInstance() {
    this.instance = new THREE.PerspectiveCamera(35, this.sizes.width / this.sizes.height);
    this.instance.position.set(6, 4, 8);
    this.scene.add(this.instance);
  }

  private setOrbitControls() {
    this.controls = new OrbitControls(this.instance, this.canvas);
    this.controls.enableDamping = true;
  }

  public resize(){
    this.instance.aspect = this.sizes.width / this.sizes.height;
    this.instance.updateProjectionMatrix();
  }

  public update(){
    this.controls.update();
  }
}
