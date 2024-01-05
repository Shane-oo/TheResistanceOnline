import {ACESFilmicToneMapping, CineonToneMapping, PCFSoftShadowMap, Scene, SRGBColorSpace, WebGLRenderer} from "three";
import {Sizes} from "./utils/sizes";
import {ResistanceGameCamera} from "./resistance-game-camera";
import {ResistanceGame} from "./resistance-game";


export class ResistanceGameRenderer {
  private readonly scene: Scene;
  private readonly sizes: Sizes;
  private readonly canvas: HTMLCanvasElement;
  private readonly renderer: WebGLRenderer
  private readonly gameCamera: ResistanceGameCamera;

  constructor(canvas: HTMLCanvasElement) {
    const resistanceGame = new ResistanceGame();

    this.scene = resistanceGame.scene;
    this.gameCamera = resistanceGame.gameCamera;
    this.sizes = resistanceGame.sizes;
    this.canvas = canvas;

    // Create Renderer
    this.renderer = new WebGLRenderer({
      canvas: this.canvas,
      antialias: true,
      powerPreference: 'high-performance'
    });
    this.configureRenderer();
  }

  resize() {
    this.renderer.setSize(this.sizes.width, this.sizes.height);
    this.renderer.setPixelRatio(this.sizes.pixelRatio);
  }

  update() {
    this.renderer.render(this.scene, this.gameCamera.perspectiveCamera)
  }

  destroy() {
    this.renderer.dispose();
  }

  private configureRenderer() {
    this.renderer.outputColorSpace = SRGBColorSpace;
    this.renderer.toneMapping = ACESFilmicToneMapping;
    this.renderer.toneMappingExposure = 1;
    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = PCFSoftShadowMap;
    this.renderer.setClearColor('#0a0b0c');
    this.renderer.setSize(this.sizes.width, this.sizes.height);
    this.renderer.setPixelRatio(this.sizes.pixelRatio);
  }

}
