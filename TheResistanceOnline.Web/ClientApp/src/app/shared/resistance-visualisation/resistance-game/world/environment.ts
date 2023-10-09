import {DirectionalLight, Scene} from "three";
import {Debug} from "../utils/debug";
import GUI from "lil-gui";

export class Environment {
    private readonly scene: Scene;
    private readonly sunLight: DirectionalLight;
    // Debug
    private readonly debugFolder?: GUI;

    constructor(scene: Scene, debug: Debug) {
        this.scene = scene;

        // SunLight
        this.sunLight = new DirectionalLight('#ffffff', 4);
        this.configureSunLight();
        this.scene.add(this.sunLight);

        // Debug
        if (debug.gui) {
            this.debugFolder = debug.gui.addFolder('environment');
            this.configureDebug();
        }
    }

    destroy() {
        this.scene.remove(this.sunLight);
        this.sunLight.dispose();
        this.debugFolder?.destroy();
    }

    private configureSunLight() {
        this.sunLight.castShadow = true;
        this.sunLight.shadow.camera.far = 15;
        this.sunLight.shadow.mapSize.set(1024, 1024);
        this.sunLight.shadow.normalBias = 0.05;
        this.sunLight.position.set(3, 3, -2.25);
    }

    private configureDebug() {
        if (this.debugFolder) {
            // Sunlight
            this.debugFolder.add(this.sunLight, 'intensity')
                .name('sunLightIntensity')
                .min(0)
                .max(10)
                .step(0.0001);
            this.debugFolder.add(this.sunLight.position, 'x')
                .name('sunLightX')
                .min(-5)
                .max(5)
                .step(0.001);
            this.debugFolder.add(this.sunLight.position, 'y')
                .name('sunLightY')
                .min(-5)
                .max(5)
                .step(0.001);
            this.debugFolder.add(this.sunLight.position, 'z')
                .name('sunLightZ')
                .min(-5)
                .max(5)
                .step(0.001);
        }
    }


}
