import {
  BufferGeometry,
  CircleGeometry,
  Mesh,
  MeshStandardMaterial,
  PlaneGeometry,
  RepeatWrapping,
  Scene,
  SRGBColorSpace
} from "three";
import {Resources, TextureResource} from "../utils/resources";
import {ResistanceGame} from "../resistance-game";

export class Floor {
  private readonly scene: Scene;
  private readonly resources: Resources;

  private readonly floorMesh: Mesh<BufferGeometry, MeshStandardMaterial>;

  constructor() {
    const resistanceGame = new ResistanceGame();
    this.scene = resistanceGame.scene;
    this.resources = resistanceGame.resources;


    this.floorMesh = this.createFloorMesh();
    this.scene.add(this.floorMesh);
  }

  destroy() {

    this.floorMesh.geometry.dispose();
    this.floorMesh.material.dispose();


    this.scene.remove(this.floorMesh);
  }

  private createFloorMesh(): Mesh<BufferGeometry, MeshStandardMaterial> {
    // 10m x 10m
    const geometry = new PlaneGeometry(10, 10, 50, 50)
    const material = new MeshStandardMaterial({});

    material.roughness = 0;

    const mesh = new Mesh(geometry, material);
    mesh.rotation.x = -Math.PI * 0.5;
    mesh.receiveShadow = true;


    return mesh;
  }
}
