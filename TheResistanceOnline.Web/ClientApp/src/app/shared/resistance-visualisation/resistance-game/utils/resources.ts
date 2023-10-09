import {GLTF, GLTFLoader} from "three/examples/jsm/loaders/GLTFLoader";
import {Texture, TextureLoader} from "three";
import {Subject} from "rxjs";

export enum ResourceType {
  CubeTexture,
  GltfModel,
  Texture
}

export enum TextureType {
  DiffuseMap,
  NormalMap
}

export interface TextureResource {
  name: string,
  texture: Texture,
  textureType: TextureType
}

export interface ModelResource {
  name: string,
  gltf: GLTF
}

export interface Resource {
  name: string,
  type: ResourceType,
  path: string | string[],
  textureType: TextureType | null
}

export class Resources {
  loadedSubject: Subject<void> = new Subject<void>();
  private readonly loaders: {
    gltfLoader: GLTFLoader,
    textureLoader: TextureLoader
  };
  private readonly sources: Resource[];
  private readonly toLoad: number;
  private loaded: number;

  constructor(sources: Resource[]) {
    this.sources = sources;

    // Setup
    this.toLoad = this.sources.length;
    this.loaded = 0;

    this.loaders = {
      gltfLoader: new GLTFLoader(),
      textureLoader: new TextureLoader()
    };

    this.startLoading();
  }

  private _models: ModelResource[] = [];

  get models(): ModelResource[] {
    return this._models;
  }

  private _textures: TextureResource[] = [];

  get textures(): TextureResource[] {
    return this._textures;
  }

  getTextureResourceByName(name: string): TextureResource {
    const index = this._textures.findIndex(r => r.name === name);
    if (index !== -1) {
      return this._textures[index];
    }
    throw new Error(`Texture Resource "${name}" not found`);
  }

  getModelResourceByName(name: string): ModelResource {
    const index = this._models.findIndex(r => r.name === name);
    if (index !== -1) {
      return this._models[index];
    }
    throw new Error(`Model Resource "${name}" not found`);
  }

  destroy() {
    for (const texture of this._textures) {
      texture.texture.dispose();
    }

    this._textures = [];
    this._models = [];
  }

  private startLoading() {
    // Load each source
    for (const source of this.sources) {
      switch (source.type) {
        case ResourceType.GltfModel:
          this.loaders.gltfLoader.load(<string>source.path,
            (gltf: GLTF) => {
              this.modelLoaded(source, gltf);
            });
          break;
        case ResourceType.Texture:
          this.loaders.textureLoader.load(<string>source.path,
            (texture: Texture) => {
              this.textureLoaded(source, texture);
            });
          break;
      }
    }
  }

  private modelLoaded(source: Resource, gltf: GLTF) {
    this._models.push({name: source.name, gltf: gltf});
    this.updateLoad();
  }

  private textureLoaded(source: Resource, texture: Texture) {
    this._textures.push({name: source.name, texture: texture, textureType: source.textureType!});
    this.updateLoad();
  }

  private updateLoad() {
    this.loaded++;
    if (this.loaded === this.toLoad) {
      // emit all loaded
      this.loadedSubject.next();
    }
  }
}
