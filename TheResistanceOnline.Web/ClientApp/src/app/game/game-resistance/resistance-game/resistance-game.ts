import {Sizes} from "./utils/sizes";
import {Time} from "./utils/time";
import {Subject, takeUntil} from "rxjs";
import {ResistanceGameCamera} from "./resistance-game-camera";
import {ResistanceGameRenderer} from "./resistance-game-renderer";
import {Scene} from "three";
import {World} from "./world/world";
import {Resources} from "./utils/resources";

import {sources} from "./sources";
import {Debug} from "./utils/debug";
import {Dispose} from "./utils/dispose";
import {Metrics} from "./utils/metrics";

export class ResistanceGame {
  private static instance: ResistanceGame;
  private readonly destroyed = new Subject<void>();
  public readonly gameCamera!: ResistanceGameCamera;
  private readonly gameRenderer!: ResistanceGameRenderer;
  // Three.js
  public readonly scene!: Scene;
  // World
  private world?: World;
  // Utils
  public readonly debug!: Debug;
  private readonly metrics!: Metrics;
  public readonly resources!: Resources;
  private readonly time!: Time;
  public readonly sizes!: Sizes;


  constructor(canvas?: HTMLCanvasElement) {

    // Singleton Pattern
    if (ResistanceGame.instance) {
      return ResistanceGame.instance;
    }
    ResistanceGame.instance = this;

    // Setup
    this.debug = new Debug();
    this.metrics = new Metrics();

    this.sizes = new Sizes();
    this.time = new Time();

    this.scene = new Scene();
    this.resources = new Resources(sources);
    this.gameCamera = new ResistanceGameCamera(canvas!);
    this.gameRenderer = new ResistanceGameRenderer(canvas!);

    this.resources.loadedSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        // Wait for all Resources before instantiating world
        this.world = new World();
      })

    // Events
    this.sizes.resizeSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        this.resize();
      });

    this.time.tickSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        this.update();
      });
  }

  destroy() {
    this.destroyed.next();

    this.world?.destroy();
    this.gameCamera.destroy();
    this.gameRenderer.destroy();
    this.resources.destroy();
    this.debug.destroy();
    this.metrics.destroy();
    this.sizes.destroy();
    this.time.destroy();
    // dispose of everything potentially left in scene
    this.scene.traverse((child) => {
      Dispose.disposeOfChild(child);
    });
  }

  setPlayers(players: string[]) {
    console.log("Setting Players");
    // for now put a cube where the player should be
    this.world?.setPlayers(players);
  }


  private resize() {
    this.gameCamera.resize();
    this.gameRenderer.resize();
  }

  private update() {
    // camera must be updated before renderer
    this.gameCamera.update();
    if (this.world) {
      this.world.update();
    }
    this.metrics.update();
    this.gameRenderer.update();
  }
}
