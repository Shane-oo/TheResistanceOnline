import EventEmitter from './EventEmitter';
import { GLTF, GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import * as THREE from 'three';
import { CubeTexture, Texture } from 'three';

export default class Resources extends EventEmitter {
  // TODO: change from any to specified interface.
  public sources: [any];
  public items: any;
  public gltfModels: { name: string; model: GLTF } [] = [];
  public textures: { name: string; texture: Texture }[] = [];
  public cubeTexture!: CubeTexture;
  public toLoad: number;
  public loaded: number;
  public loaders!: {
    gltfLoader: GLTFLoader,
    textureLoader: THREE.TextureLoader,
    cubeTextureLoader: THREE.CubeTextureLoader
  };

  constructor(sources: [any]) {
    super();
    // Options
    this.sources = sources;

    // Setup
    this.items = {};

    // obvs has fucked up somewhere         ?????
    this.toLoad = this.sources[0].length;
    console.log(this.toLoad);
    this.loaded = 0;

    this.setLoaders();
    this.startLoading();
  }

  private setLoaders() {
    let gltfLoader = new GLTFLoader();
    let textureLoader = new THREE.TextureLoader();
    let cubeTextureLoader = new THREE.CubeTextureLoader();
    this.loaders = {gltfLoader, textureLoader, cubeTextureLoader};

  }

  private startLoading() {
    // Load each source
    for(const source of this.sources) {
      for(const insideSource of source) {
        if(insideSource.type == 'gltfModel') {
          this.loaders.gltfLoader.load(insideSource.path, (file) => {
            this.sourceLoaded(insideSource.name, file, undefined, undefined);
          });
        } else if(insideSource.type == 'texture') {
          this.loaders.textureLoader.load(insideSource.path, (file) => {
            this.sourceLoaded(insideSource.name, undefined, file, undefined);
          });
        } else if(insideSource.type == 'cubeTexture') {
          this.loaders.cubeTextureLoader.load(insideSource.path, (file) => {
            this.sourceLoaded(insideSource.name, undefined, undefined, file);
          });
        }
      }
    }
  }

  private sourceLoaded(fileName: string, gltfFile?: GLTF, textureFile?: Texture, cubeTextureFile?: CubeTexture) {
    if(gltfFile) {
      this.gltfModels.push({name: fileName, model: gltfFile});
      this.loaded++;
    }
    if(textureFile) {
      this.textures.push({name: fileName, texture: textureFile});
      this.loaded++;
    }

    if(cubeTextureFile) {
      this.cubeTexture = cubeTextureFile;
      this.loaded++;
    }
    if(this.loaded === this.toLoad) {
      this.trigger('ready');
    }

  }

}
