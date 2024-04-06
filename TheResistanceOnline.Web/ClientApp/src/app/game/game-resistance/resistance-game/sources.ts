import {Resource, ResourceType} from "./utils/resources";
import {Vector3} from "three";

export const sources: Resource[] = [
  {
    name: 'ethnocentricFont',
    type: ResourceType.Font,
    path: 'assets/fonts/ethnocentric_font.json',
    scale: null,
    textureType: null
  },
  {
    name: 'cyberPunkTable',
    type: ResourceType.GltfModel,
    path: 'assets/models/ResistanceGame/CyberpunkTable.glb',
    scale: null,

    textureType: null
  },
  {
    name: 'soldiers',
    type: ResourceType.GltfModel,
    path: 'assets/models/ResistanceGame/soldiers.glb',
    scale: new Vector3(0.1,0.1,0.1),
    textureType: null
  }

];
