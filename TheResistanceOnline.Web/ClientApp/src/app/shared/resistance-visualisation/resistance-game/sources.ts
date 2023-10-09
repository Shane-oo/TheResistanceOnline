import {Resource, ResourceType, TextureType} from "./utils/resources";

export const sources: Resource[] = [
  {
    name: 'testGltfModel',
    type: ResourceType.GltfModel,
    path: ['assets/models/Fox/glTF-Binary/Fox.glb'],
    textureType: null
  },
  {
    name: 'floorDiffuseTexture',
    type: ResourceType.Texture,
    path: 'assets/textures/dirt/color.jpg',
    textureType: TextureType.DiffuseMap
  },
  {
    name: 'floorNormalTexture',
    type: ResourceType.Texture,
    path: 'assets/textures/dirt/normal.jpg',
    textureType: TextureType.NormalMap
  },
];


