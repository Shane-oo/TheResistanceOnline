import {MathUtils, PerspectiveCamera, Scene} from 'three';
import {OrbitControls} from "three/examples/jsm/controls/OrbitControls";
import {Sizes} from "./utils/sizes";
import {Debug} from "./utils/debug";
import GUI from "lil-gui";

export class ResistanceGameCamera {

  // Values
  private readonly fov = 32;
  private readonly planeAspectRatio = 2.75;

  private readonly _perspectiveCamera: PerspectiveCamera;
  private readonly sizes: Sizes;
  private readonly orbitControls: OrbitControls;
  private readonly scene: Scene;
  private readonly canvas: HTMLCanvasElement;

  // Debug
  private readonly debugFolder?: GUI;

  constructor(scene: Scene, sizes: Sizes, canvas: HTMLCanvasElement, debug: Debug) {
    this.sizes = sizes;
    this.scene = scene;
    this.canvas = canvas;
    console.log(this.sizes.width)
    console.log(this.sizes.height)
    // Perspective Camera
    this._perspectiveCamera = new PerspectiveCamera(this.fov,
      this.sizes.width / this.sizes.height,
      0.1,
      100);
    this.configureCamera();
    this.scene.add(this._perspectiveCamera);

    // Orbit Controls
    this.orbitControls = new OrbitControls(this._perspectiveCamera, this.canvas);
    this.orbitControls.enableDamping = true;
    this.orbitControls.enabled = false;

    // Debug
    if (debug.gui) {
      this.debugFolder = debug.gui.addFolder('Camera');
      this.configureDebug();
    }
  }

  get perspectiveCamera(): PerspectiveCamera {
    return this._perspectiveCamera;
  }

  resize() {
    this._perspectiveCamera.aspect = this.sizes.width / this.sizes.height;

    if (this._perspectiveCamera.aspect > this.planeAspectRatio) {
      // window too large
      this._perspectiveCamera.fov = this.fov;
    } else {
      // window too narrow
      const cameraHeight = Math.tan(MathUtils.degToRad(this.fov / 2));
      const ratio = this._perspectiveCamera.aspect / this.planeAspectRatio;
      const newCameraHeight = cameraHeight / ratio;
      this._perspectiveCamera.fov = MathUtils.radToDeg(Math.atan(newCameraHeight)) * 2;
    }


    this._perspectiveCamera.updateProjectionMatrix();
  }

  update() {
    this.orbitControls.update();

  }

  destroy() {
    this.scene.remove(this._perspectiveCamera);
    this.orbitControls.dispose();
  }

  private configureCamera() {
    this._perspectiveCamera.position.set(0, 5.2, 0);
    this.resize();
  }

  // Debug
  private configureDebug() {
    if (this.debugFolder) {
      this.debugFolder.add(this._perspectiveCamera, 'fov')
        .name('fov')
        .min(-1000)
        .max(1000)
        .step(0.001)
        .onChange(() => this._perspectiveCamera.updateProjectionMatrix());

      this.debugFolder.add(this._perspectiveCamera.position, 'x')
        .name('cameraX')
        .min(-100)
        .max(100)
        .step(0.001);
      this.debugFolder.add(this._perspectiveCamera.position, 'y')
        .name('cameraY')
        .min(-100)
        .max(100)
        .step(0.001);
      this.debugFolder.add(this._perspectiveCamera.position, 'z')
        .name('cameraZ')
        .min(-100)
        .max(100)
        .step(0.001);
    }

  }
}
