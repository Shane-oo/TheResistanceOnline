import {
  BufferGeometry,
  Color,
  Mesh,
  MeshStandardMaterial,
  PerspectiveCamera,
  Raycaster,
  Scene,
  Vector2,
  Vector3
} from "three";
import {ResistanceGame} from "../resistance-game";
import {Sizes} from "../utils/sizes";

export class RayCasting {
  private readonly raycaster: Raycaster;
  private readonly sizes: Sizes;
  private readonly camera: PerspectiveCamera;
  private mouse: Vector2 = new Vector2();

  constructor() {
    const resistanceGame = new ResistanceGame();
    this.sizes = resistanceGame.sizes;
    this.camera = resistanceGame.gameCamera.perspectiveCamera;

    this.raycaster = new Raycaster();
    this.raycaster.camera = this.camera;

    console.log(this.raycaster.far)
    // Click Event
    window.addEventListener('mousedown', this.onMouseUp);
  }

  private _objectsToTest: Mesh[] = [];

  set objectsToTest(objects: Mesh[]) {
    this._objectsToTest = objects;
  }

  destroy() {
    window.removeEventListener('mouseup', this.onMouseUp);
  }

  private onMouseUp = (event: MouseEvent) => {
    if (this._objectsToTest.length > 0) {
      console.log("mouse up", event)
      this.mouse.x = (event.offsetX / this.sizes.width) * 2 - 1;
      this.mouse.y = -(event.offsetY / this.sizes.height) * 2 + 1;

      console.log(this.mouse);

      this.raycaster.setFromCamera(this.mouse, this.camera);

      const intersects = this.raycaster.intersectObjects(this._objectsToTest);
      console.log(intersects)
      const intersection = intersects[0] ?? null;
      if (intersection) {
        console.log("i clicked on", intersection.object)
        const mesh = intersection.object as Mesh<BufferGeometry, MeshStandardMaterial>;
        mesh.material.color = new Color('pink');

        // todo shane i am here
        // emit what objects they picked to be on the mission
      }
    }


  }

}
