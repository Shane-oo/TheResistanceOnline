import ResistanceGame from './ResistanceGame';
import Sizes from './Utils/Sizes';
import * as THREE from 'three';
import Camera from './Camera';

export default class Renderer {
  public resistanceGame: ResistanceGame;
  public canvas: HTMLCanvasElement;
  public sizes: Sizes;
  public scene: THREE.Scene;
  public camera: Camera;
  public instance: THREE.WebGLRenderer = new THREE.WebGLRenderer();

  constructor() {

    this.resistanceGame = new ResistanceGame();
    this.canvas = this.resistanceGame.canvas;
    this.sizes = this.resistanceGame.sizes;
    this.scene = this.resistanceGame.scene;
    this.camera = this.resistanceGame.camera;


    this.setInstance();


  }

  public resize() {
    this.instance.setSize(this.sizes.width, this.sizes.height);
    this.instance.setPixelRatio(Math.min(window.devicePixelRatio, 2));
  }

  public update() {

    this.instance.render(this.scene, this.camera.instance);
  }

  private setInstance() {

    this.instance = new THREE.WebGLRenderer({
                                              canvas: this.canvas,
                                              antialias: true
                                            });


    this.instance.physicallyCorrectLights = true;
    this.instance.outputEncoding = THREE.sRGBEncoding;
    this.instance.toneMapping = THREE.CineonToneMapping;
    this.instance.toneMappingExposure = 1.75;
    this.instance.shadowMap.enabled = true;
    this.instance.shadowMap.type = THREE.PCFSoftShadowMap;
    this.instance.setClearColor('#211d20');
    this.instance.setSize(this.sizes.width, this.sizes.height);
    this.instance.setPixelRatio(Math.min(window.devicePixelRatio, 2));
  }
}
