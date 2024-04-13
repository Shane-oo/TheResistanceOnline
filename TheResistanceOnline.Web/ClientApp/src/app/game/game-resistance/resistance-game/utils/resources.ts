import {GLTF, GLTFLoader} from "three/examples/jsm/loaders/GLTFLoader";
import {Texture, TextureLoader, Vector3} from "three";
import {Subject} from "rxjs";
import {Font, FontLoader} from "three/examples/jsm/loaders/FontLoader";
import {Dispose} from "./dispose";
import {EXRLoader} from "three/examples/jsm/loaders/EXRLoader";
import {DRACOLoader} from "three/examples/jsm/loaders/DRACOLoader";

export enum ResourceType {
  CubeTexture,
  GltfModel,
  Texture,
  Font
}

export enum TextureType {
  DiffuseMap,
  NormalMap,
  HDREnvironmentMap
}

export interface TextureResource {
  name: string;
  texture: Texture;
  textureType: TextureType;
}

export interface ModelResource {
  name: string;
  gltf: GLTF;
}

export interface FontResource {
  name: string;
  font: Font;
}

export interface Resource {
  name: string;
  type: ResourceType;
  path: string;
  scale: Vector3 | null;
  textureType: TextureType | null;
}

export class Resources {
  loadedSubject: Subject<void> = new Subject<void>();
  private readonly loaders: {
    GLTFLoader: GLTFLoader,
    textureLoader: TextureLoader,
    fontLoader: FontLoader,
    EXRLoader: EXRLoader
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
      GLTFLoader: new GLTFLoader(),
      textureLoader: new TextureLoader(),
      fontLoader: new FontLoader(),
      EXRLoader: new EXRLoader()
    };

    const dracoLoader = new DRACOLoader();
    dracoLoader.setDecoderPath('/assets/loaders/draco/gltf/');
    this.loaders.GLTFLoader.setDRACOLoader(dracoLoader);

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

  private _fonts: FontResource[] = [];

  get fonts(): FontResource[] {
    return this._fonts;
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

  getFontByName(name: string): FontResource {
    const index = this._fonts.findIndex(r => r.name === name);
    if (index !== -1) {
      return this._fonts[index];
    }
    throw new Error(`Font Resource "${name}" not found`);
  }

  destroy() {
    for (const texture of this._textures) {
      texture.texture.dispose();
    }
    for (const model of this._models) {
      model.gltf.scene.traverse((child) => {
        Dispose.disposeOfChild(child);
      });
    }

    this._textures = [];
    this._models = [];
  }

  private startLoading() {
    // Load each source
    for (const source of this.sources) {
      switch (source.type) {
        case ResourceType.GltfModel:
          this.loaders.GLTFLoader.load(source.path,
            (gltf: GLTF) => {
              this.modelLoaded(source, gltf);
            });
          break;
        case ResourceType.Texture:
          if (source.textureType === TextureType.HDREnvironmentMap) {
            this.loaders.EXRLoader.load(source.path, (texture: Texture) => {
              this.textureLoaded(source, texture);
            })
          } else {
            this.loaders.textureLoader.load(source.path,
              (texture: Texture) => {
                this.textureLoaded(source, texture);
              });
          }
          break;
        case ResourceType.Font:
          this.loaders.fontLoader.load(source.path,
            (font: Font) => {
              this.fontLoaded(source, font);
            });
      }
    }
  }

  private fontLoaded(source: Resource, font: Font) {
    this._fonts.push({name: source.name, font: font});
    this.updateLoad();
  }

  private modelLoaded(source: Resource, gltf: GLTF) {
    gltf.scene.traverse((child) => {
      child.castShadow = true;
      child.receiveShadow = true;
      if (source.scale) {
        console.log('scaling', source.scale)
        child.scale.set(source.scale.x, source.scale.y, source.scale.z);
      }
    });


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
