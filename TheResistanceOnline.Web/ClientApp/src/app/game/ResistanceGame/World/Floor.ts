import ResistanceGame from '../ResistanceGame';
import * as THREE from 'three';
import Environment from './Environment';
import Resources from '../Utils/Resources';

export default class Floor {
  public resistanceGame: ResistanceGame;
  public scene: THREE.Scene;
  public environment!: Environment;
  public resources: Resources;
  public geometry!: THREE.CircleGeometry;
  public grassColorTexture!: THREE.Texture;
  public grassNormalTexture!: THREE.Texture;
  public material!: THREE.MeshStandardMaterial;
  public mesh!:THREE.Mesh;
  constructor() {
    this.resistanceGame = new ResistanceGame();
    this.scene = this.resistanceGame.scene;
    this.resources = this.resistanceGame.resources;

    this.setGeometry();
    this.setTextures();
    this.setMaterial();
    this.setMesh();
  }

  private setGeometry() {
    this.geometry = new THREE.CircleGeometry(5, 64);

  }

  private setTextures() {
    let grassColorTexture = this.resources.textures.find(obj => {
      return obj.name === 'grassColorTexture';
    });

    if(grassColorTexture) {
      this.grassColorTexture = grassColorTexture.texture;
      this.grassColorTexture.encoding = THREE.sRGBEncoding;
      this.grassColorTexture.repeat.set(1.5, 1.5);
      this.grassColorTexture.wrapS = THREE.RepeatWrapping;
      this.grassColorTexture.wrapT = THREE.RepeatWrapping;
    }
    let grassNormalTexture = this.resources.textures.find(obj => {
      return obj.name === 'grassNormalTexture';
    });

    if(grassNormalTexture) {
      this.grassNormalTexture = grassNormalTexture.texture;
      this.grassNormalTexture.repeat.set(1.5, 1.5);
      this.grassNormalTexture.wrapS = THREE.RepeatWrapping;
      this.grassNormalTexture.wrapT = THREE.RepeatWrapping;
    }

  }

  private setMaterial() {
    this.material = new THREE.MeshStandardMaterial({
                                                      map: this.grassColorTexture,
                                                      normalMap: this.grassNormalTexture
                                                    });
  }

  private setMesh() {
    this.mesh = new THREE.Mesh(this.geometry,this.material);
    this.mesh.rotation.x = -Math.PI * 0.5;
    this.mesh.receiveShadow = true;
    this.scene.add(this.mesh);
  }

}
