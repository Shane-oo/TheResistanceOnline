import {Scene} from "three";
import {Environment} from "./environment";
import {Resources} from "../utils/resources";
import {Floor} from "./floor";
import {Fox} from "./fox";
import {Time} from "../utils/time";
import {Debug} from "../utils/debug";

export class World {
    private readonly scene: Scene;
    private readonly environment: Environment;
    private readonly fox: Fox;
    private readonly floor: Floor;

    // Utils
    private readonly resources: Resources;
    private readonly debug: Debug;

    constructor(scene: Scene, time: Time, resources: Resources, debug: Debug) {
        this.scene = scene;
        this.resources = resources;
        this.debug = debug;

        // Add all children of World
        this.fox = new Fox(this.scene, time, this.resources, this.debug);
        this.floor = new Floor(this.scene, this.resources);

        this.environment = new Environment(this.scene, this.debug);
    }

    update() {
        this.fox.update();
    }

    destroy() {
        this.environment.destroy();
        this.floor.destroy();
        this.fox.destroy();
    }

}
