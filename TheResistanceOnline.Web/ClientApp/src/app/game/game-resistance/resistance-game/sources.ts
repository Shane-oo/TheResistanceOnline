import {Resource, ResourceType, TextureType} from "./utils/resources";

export const sources: Resource[] = [
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
  {
    name: 'missionTeamPiece',
    type: ResourceType.GltfModel,
    path: 'assets/models/ResistanceGame/MissionTeamPiece.glb',
    textureType: null
  }
];


