import {BufferGeometry, Mesh, MeshStandardMaterial, PerspectiveCamera, Raycaster, Vector2} from "three";
import {ResistanceGame} from "./resistance-game";
import {Sizes} from "./utils/sizes";
import {Subject} from "rxjs";

export class ResistanceGameRaycasting {
  objectClickedSubject: Subject<string> = new Subject<string>();

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

    // Click Event
    window.addEventListener('mousedown', this.onMouseUp);
  }

  private _selectableObjects: Mesh[] = [];

  set selectableObjects(objects: Mesh[]) {
    this._selectableObjects = objects;
  }

  destroy() {
    window.removeEventListener('mouseup', this.onMouseUp);
  }

  private onMouseUp = (event: MouseEvent) => {
    if (this._selectableObjects.length > 0) {
      this.mouse.x = (event.offsetX / this.sizes.width) * 2 - 1;
      this.mouse.y = -(event.offsetY / this.sizes.height) * 2 + 1;

      this.raycaster.setFromCamera(this.mouse, this.camera);

      const intersects = this.raycaster.intersectObjects(this._selectableObjects);
      const intersection = intersects[0] ?? null;
      if (intersection) {
        const mesh = intersection.object as Mesh<BufferGeometry, MeshStandardMaterial>;
        this.objectClickedSubject.next(mesh.name);
      }
    }


  }

}