import {ACESFilmicToneMapping, PCFSoftShadowMap, Scene, SRGBColorSpace, WebGLRenderer} from "three";
import {Sizes} from "./utils/sizes";
import {ResistanceGameCamera} from "./resistance-game-camera";
import {ResistanceGame} from "./resistance-game";


export class ResistanceGameRenderer {
  private readonly scene: Scene;
  private readonly sizes: Sizes;
  private readonly canvas: HTMLCanvasElement;
  private readonly _renderer: WebGLRenderer
  private readonly gameCamera: ResistanceGameCamera;

  constructor(canvas: HTMLCanvasElement) {
    const resistanceGame = new ResistanceGame();

    this.scene = resistanceGame.scene;
    this.gameCamera = resistanceGame.gameCamera;
    this.sizes = resistanceGame.sizes;
    this.canvas = canvas;

    // Create Renderer
    this._renderer = new WebGLRenderer({
      canvas: this.canvas,
      antialias: true,
      powerPreference: 'high-performance'
    });
    this.configureRenderer();
  }

  get renderer(): WebGLRenderer {
    return this._renderer;
  }

  resize() {
    this._renderer.setSize(this.sizes.width, this.sizes.height);
    this._renderer.setPixelRatio(this.sizes.pixelRatio);
  }

  update() {
    this._renderer.render(this.scene, this.gameCamera.perspectiveCamera)
  }

  destroy() {
    this._renderer.dispose();
  }

  private configureRenderer() {
    this._renderer.outputColorSpace = SRGBColorSpace;
    this._renderer.toneMapping = ACESFilmicToneMapping;
    this._renderer.toneMappingExposure = 2;
    this._renderer.shadowMap.enabled = true;
    this._renderer.shadowMap.type = PCFSoftShadowMap;
    this._renderer.setClearColor('#0a0b0c');
    this._renderer.setSize(this.sizes.width, this.sizes.height);
    this._renderer.setPixelRatio(this.sizes.pixelRatio);
  }

}
