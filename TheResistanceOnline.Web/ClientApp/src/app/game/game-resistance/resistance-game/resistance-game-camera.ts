import {MathUtils, PerspectiveCamera, Scene, Vector3} from 'three';
import {OrbitControls} from "three/examples/jsm/controls/OrbitControls";
import {Sizes} from "./utils/sizes";
import GUI from "lil-gui";

import {gsap} from "gsap";

import {ResistanceGame} from "./resistance-game";
import {Position} from "./resistance-game-models";

export class ResistanceGameCamera {
  // Constants
  private readonly fov = 35;
  private readonly planeAspectRatio = 2.75;

  private readonly _perspectiveCamera: PerspectiveCamera;
  private readonly sizes: Sizes;
  private readonly orbitControls: OrbitControls;
  private readonly scene: Scene;
  private readonly canvas: HTMLCanvasElement;

  // Debug
  private readonly debugFolder?: GUI;

  constructor(canvas: HTMLCanvasElement) {
    const resistanceGame = new ResistanceGame();
    this.sizes = resistanceGame.sizes;
    this.scene = resistanceGame.scene;
    this.canvas = canvas;

    // Perspective Camera
    this._perspectiveCamera = this.createCamera();
    this.resize();
    this.scene.add(this._perspectiveCamera);

    // Orbit Controls
    this.orbitControls = this.createOrbitControls();

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder('Camera');
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

  playStartGameAnimation(playerPosition: Position) {
    this._perspectiveCamera.lookAt(new Vector3(playerPosition.x, playerPosition.y + 1.5, playerPosition.z));
    gsap.to(this._perspectiveCamera.position, {
      duration: 8,
      x: playerPosition.x,
      y: playerPosition.y + 1.5,
      z: playerPosition.z
    });
  }

  update() {
    this.orbitControls.update();
  }

  destroy() {
    this.scene.remove(this._perspectiveCamera);
    this.orbitControls.dispose();
  }

  private createCamera(): PerspectiveCamera {
    const camera = new PerspectiveCamera(this.fov,
      this.sizes.width / this.sizes.height,
      0.1,
      100);
    camera.position.set(0, 7.5, 2);
    return camera;
  }

  private createOrbitControls(): OrbitControls {
    const controls = new OrbitControls(this._perspectiveCamera, this.canvas);
    controls.enableDamping = true;
    controls.enabled = true;
    controls.maxPolarAngle = Math.PI / 4;
    controls.enableZoom = false;
    controls.enablePan = false;

    return controls;
  }

  // Debug
  private configureDebug() {
    if (this.debugFolder) {
      this.debugFolder.add(this._perspectiveCamera, 'fov')
        .name('fov')
        .min(0)
        .max(100)
        .step(0.01)
        .onChange(() => this._perspectiveCamera.updateProjectionMatrix());

      this.debugFolder.add(this._perspectiveCamera.position, 'x')
        .name('cameraX')
        .min(-100)
        .max(100)
        .step(0.01);
      this.debugFolder.add(this._perspectiveCamera.position, 'y')
        .name('cameraY')
        .min(-100)
        .max(100)
        .step(0.01);
      this.debugFolder.add(this._perspectiveCamera.position, 'z')
        .name('cameraZ')
        .min(-100)
        .max(100)
        .step(0.01);
    }

  }
}
