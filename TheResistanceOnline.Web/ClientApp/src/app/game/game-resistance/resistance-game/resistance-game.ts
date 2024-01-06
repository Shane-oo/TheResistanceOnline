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
import {VoteSubmittedModel} from "../game-resistance.models";


export class ResistanceGame {
  private static instance: ResistanceGame | null;
  objectClickedSubject: Subject<string> = new Subject<string>();

  private readonly _gameCamera!: ResistanceGameCamera;
  private readonly _scene!: Scene;
  private readonly _debug!: Debug;
  private readonly _resources!: Resources;
  private readonly _sizes!: Sizes;
  private readonly _rayCasting!: RayCasting;
  private readonly destroyed = new Subject<void>();
  private readonly _gameRenderer!: ResistanceGameRenderer;
  private readonly _metrics!: Metrics;
  private readonly _time!: Time;

  private readonly players?: string[];

  constructor(canvas?: HTMLCanvasElement) {

    // Singleton Pattern
    if (ResistanceGame.instance) {
      return ResistanceGame.instance;
    }
    ResistanceGame.instance = this;

    // Setup
    this._debug = new Debug();
    this._metrics = new Metrics();

    this._sizes = new Sizes();
    this._time = new Time();

    this._scene = new Scene();
    this._resources = new Resources(sources);
    this._gameCamera = new ResistanceGameCamera(canvas!);
    this._gameRenderer = new ResistanceGameRenderer(canvas!);
    this._rayCasting = new RayCasting();

    // Events
    this._resources.loadedSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        // Wait for all Resources before instantiating world
        this._world = new World();
      })

    this._sizes.resizeSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        this.resize();
      });

    this._time.tickSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe(() => {
        this.update();
      });

    this._rayCasting.objectClickedSubject
      .pipe(takeUntil(this.destroyed))
      .subscribe((name: string) => {
        this.objectClickedSubject.next(name);
      });
  }

  get gameCamera(): ResistanceGameCamera {
    return this._gameCamera;
  }

  get rayCasting(): RayCasting {
    return this._rayCasting;
  }

  get metrics(): Metrics {
    return this._metrics;
  }

  get time(): Time {
    return this._time;
  }

  get sizes(): Sizes {
    return this._sizes;
  }

  get gameRenderer(): ResistanceGameRenderer {
    return this._gameRenderer;
  }

  get scene(): Scene {
    return this._scene;
  }

  get resources(): Resources {
    return this._resources;
  }

  get debug(): Debug {
    return this._debug;
  }

  private _world?: World;

  get world(): World | undefined {
    return this._world;
  }

  destroy() {
    this.destroyed.next();

    this._world?.destroy();
    this._gameCamera.destroy();
    this._gameRenderer.destroy();
    this._resources.destroy();
    this._debug.destroy();
    this._metrics.destroy();
    this._sizes.destroy();
    this._time.destroy();
    // dispose of everything potentially left in scene
    this._scene.traverse((child) => {
      Dispose.disposeOfChild(child);
    });

    ResistanceGame.instance = null;
  }

  setPlayers(players: string[]) {
    // for now put a cube where the player should be
    this._world?.setPlayers(players);
  }

  setMissionLeader(player: string) {
    this._world?.setMissionLeader(player);
  }

  setMissionBuildPhase(missionMembers: number) {
    // todo set a timeout and say if this user does not pick missionMembers in time
    // pick at random for them 3 minutes max
    // display timer maybe?
    this._world?.setMissionBuildPhase(missionMembers);
  }

  addMissionTeamMember(player: string) {
    this._world?.addMissionTeamMember(player);
  }

  removeMissionTeamMember(player: string) {
    this._world?.removeMissionTeamMember(player);
  }

  //TODO
  startVotePhase(missionTeamMembers: string[]) {
    console.log("STarting Vote Phase", missionTeamMembers);
    // show who is on missionTeam
    // show vote pieces, make them selectable

  }

  // TODO
  playerVoted(playerVote: VoteSubmittedModel) {
    console.log(playerVote.playerName, "Voted ", playerVote.accepted);
    // show what this player voted
  }


  private resize() {
    this._gameCamera.resize();
    this._gameRenderer.resize();
  }

  private update() {
    // camera must be updated before renderer
    this._gameCamera.update();
    if (this._world) {
      this._world.update();
    }
    this._metrics.update();
    this._gameRenderer.update();
  }
}
