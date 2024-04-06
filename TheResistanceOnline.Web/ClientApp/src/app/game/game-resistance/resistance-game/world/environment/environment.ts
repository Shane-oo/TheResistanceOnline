import {
  DirectionalLight, DirectionalLightShadow,
  EquirectangularReflectionMapping,
  EquirectangularRefractionMapping,
  PMREMGenerator,
  Scene
} from "three";
import GUI from "lil-gui";
import {ResistanceGame} from "../../resistance-game";
import {Resources} from "../../utils/resources";
import {ResistanceGameRenderer} from "../../resistance-game-renderer";

export class Environment {
  private readonly scene: Scene;
  private readonly resources: Resources;
  private readonly sunLight: DirectionalLight;
  private readonly gameRenderer: ResistanceGameRenderer;
  // Debug
  private readonly debugFolder?: GUI;

  constructor() {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;
    this.gameRenderer = resistanceGame.gameRenderer;

    // SunLight
    this.sunLight = this.createSunLight();
    this.scene.add(this.sunLight);

    //this.addSceneBackground();

    // Debug
    if (resistanceGame.debug.gui) {
      this.debugFolder = resistanceGame.debug.gui.addFolder('environment');
      this.configureDebug();
    }
  }

  destroy() {
    this.scene.remove(this.sunLight);
    this.sunLight.dispose();
    this.debugFolder?.destroy();
  }

  private createSunLight(): DirectionalLight {
    const sunlight = new DirectionalLight('#ffffff', Math.PI);
    sunlight.castShadow = true;
    sunlight.shadow.camera.far = 30;
    sunlight.shadow.mapSize.set(1024, 1024);
    sunlight.shadow.normalBias = 0.05;
    // 20 m high
    sunlight.position.set(0, 20, -2.25);


    return sunlight;
  }

  private addSceneBackground() {


    const environmentMap = this.resources.getTextureResourceByName('broadwayEnvironmentMap').texture;
    environmentMap.mapping = EquirectangularRefractionMapping;

    this.scene.environment = environmentMap;
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
