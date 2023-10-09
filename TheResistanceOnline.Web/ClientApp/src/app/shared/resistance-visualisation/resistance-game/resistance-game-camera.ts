import {PerspectiveCamera, Scene} from 'three';
import {OrbitControls} from "three/examples/jsm/controls/OrbitControls";
import {Sizes} from "./utils/sizes";

export class ResistanceGameCamera {
    private readonly _perspectiveCamera: PerspectiveCamera;
    private readonly sizes: Sizes;
    private readonly orbitControls: OrbitControls;
    private readonly scene: Scene;
    private readonly canvas: HTMLCanvasElement;

    constructor(scene: Scene, sizes: Sizes, canvas: HTMLCanvasElement) {
        this.sizes = sizes;
        this.scene = scene;
        this.canvas = canvas;

        // Perspective Camera
        this._perspectiveCamera = new PerspectiveCamera(35,
            this.sizes.width / this.sizes.height,
            0.1,
            100);
        this._perspectiveCamera.position.set(6, 4, 8);
        this.scene.add(this._perspectiveCamera);

        // Orbit Controls
        this.orbitControls = new OrbitControls(this._perspectiveCamera, this.canvas);
        this.orbitControls.enableDamping = true;
    }

    get perspectiveCamera(): PerspectiveCamera {
        return this._perspectiveCamera;
    }

    resize() {
        this._perspectiveCamera.aspect = this.sizes.width / this.sizes.height;
        this._perspectiveCamera.updateProjectionMatrix();
    }

    update() {
        this.orbitControls.update();

    }

    destroy() {
        this.scene.remove(this._perspectiveCamera);
        this.orbitControls.dispose();
    }
}
