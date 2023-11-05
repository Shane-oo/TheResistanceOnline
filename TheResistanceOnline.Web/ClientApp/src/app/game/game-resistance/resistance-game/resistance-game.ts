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
import {RayCasting} from "./world/raycasting";


export class ResistanceGame {
  objectClickedSubject: Subject<string> = new Subject<string>();

  private static instance: ResistanceGame | null;
  public readonly gameCamera!: ResistanceGameCamera;
  public readonly scene!: Scene;
  public readonly debug!: Debug;
  public readonly resources!: Resources;
  public readonly sizes!: Sizes;
  public readonly rayCasting!: RayCasting;
  private readonly destroyed = new Subject<void>();
  private readonly gameRenderer!: ResistanceGameRenderer;
  private world?: World;
  private readonly metrics!: Metrics;
  private readonly time!: Time;

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
    this.rayCasting = new RayCasting();

    // Events
    this.resources.loadedSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        // Wait for all Resources before instantiating world
        this.world = new World();
      })

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

    this.rayCasting.objectClickedSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe((name: string) => {
        this.objectClickedSubject.next(name);
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

    ResistanceGame.instance = null;
  }

  setPlayers(players: string[]) {
    // for now put a cube where the player should be
    this.world?.setPlayers(players);
  }

  setMissionLeader(player: string) {
    this.world?.setMissionLeader(player);
  }

  setMissionBuildPhase(missionMembers: number) {
    // todo set a timeout and say if this user does not pick missionMembers in time
    // pick at random for them 3 minutes max
    // display timer maybe?
    this.world?.setMissionBuildPhase(missionMembers);
  }

  addMissionTeamMember(player: string) {
    this.world?.addMissionTeamMember(player);
  }

  removeMissionTeamMember(player:string){
    this.world?.removeMissionTeamMember(player);
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
