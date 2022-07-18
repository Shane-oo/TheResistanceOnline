import ResistanceGame from '../ResistanceGame';
import * as THREE from 'three';
import Resources from '../Utils/Resources';

export default class Environment {
  public resistanceGame: ResistanceGame;
  public scene: THREE.Scene;
  public sunLight!: THREE.DirectionalLight;
  public resources: Resources;

  constructor() {
    this.resistanceGame = new ResistanceGame();
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
  }

  private setEnvironmentMap() {
    this.resources.cubeTexture.encoding = THREE.sRGBEncoding;
    this.scene.environment = this.resources.cubeTexture;

    this.updateAllMaterials(this.resources.cubeTexture);
  }

  private updateAllMaterials(environmentMap: THREE.CubeTexture) {
    this.scene.traverse((child) => {
      if(child instanceof THREE.Mesh && child.material instanceof THREE.MeshStandardMaterial) {
        child.material.envMap = environmentMap;
        // TODO figure out what this value should be
        child.material.envMapIntensity = 0.4;
        child.material.needsUpdate = true;
      }
    });
  }

}
