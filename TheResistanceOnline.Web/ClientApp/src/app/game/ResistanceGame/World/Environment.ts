import ResistanceGame from '../ResistanceGame';
import * as THREE from 'three';
import Resources from '../Utils/Resources';
import Debug from '../Utils/Debug';
import { CubeTexture } from 'three';

export default class Environment {
  public resistanceGame: ResistanceGame;
  public scene: THREE.Scene;
  public sunLight!: THREE.DirectionalLight;
  public resources: Resources;
  public debug: Debug;
  public envMap!: { intensity: number, environmentMapTexture: CubeTexture };
  // TODO what is debug folder type
  public debugFolder: any;

  constructor() {
    this.resistanceGame = new ResistanceGame();
    this.debug = this.resistanceGame.debug;

    // Debug
    if(this.debug.active) {
      this.debugFolder = this.debug.gui.addFolder('environment');
    }
    this.scene = this.resistanceGame.scene;
    this.resources = this.resistanceGame.resources;
    // Setup
    this.setSunLight();
    this.setEnvironmentMap();
  }

  private setSunLight() {
    this.sunLight = new THREE.DirectionalLight('#ffffff', 3);
    this.sunLight.position.set(3.5, 2, -1.25);
    this.sunLight.castShadow = true;
    this.sunLight.shadow.camera.far = 15;
    this.sunLight.shadow.mapSize.set(1024, 1024);
    this.sunLight.shadow.normalBias = 0.05;
    this.scene.add(this.sunLight);

    // Debug
    if(this.debug.active) {
      this.debugFolder.add(this.sunLight, 'intensity')
          .name('sunLightIntensity')
          .min(0)
          .max(10)
          .step(0.001);
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

  private setEnvironmentMap() {
    this.resources.cubeTexture.encoding = THREE.sRGBEncoding;
    this.scene.environment = this.resources.cubeTexture;
    let intensity = 0.4;
    let environmentMapTexture = this.resources.cubeTexture;
    this.envMap = {intensity, environmentMapTexture};
    this.updateAllMaterials();

    // Debug
    if(this.debug.active) {
      this.debugFolder.add(this.envMap, 'intensity')
          .name('envMapIntensity')
          .min(0)
          .max(4)
          .step(0.001)
          .onChange(() => {
            this.updateAllMaterials();
          });
    }
  }

  private updateAllMaterials() {
    this.scene.traverse((child) => {
      if(child instanceof THREE.Mesh && child.material instanceof THREE.MeshStandardMaterial) {
        child.material.envMap = this.envMap.environmentMapTexture;
        // TODO figure out what this value should be
        child.material.envMapIntensity = this.envMap.intensity;
        child.material.needsUpdate = true;
      }
    });
  }

}
