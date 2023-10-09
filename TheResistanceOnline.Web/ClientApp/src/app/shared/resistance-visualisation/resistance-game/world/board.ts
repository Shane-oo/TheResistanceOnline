import {Group, Mesh, Scene} from "three";
import {ModelResource, Resources} from "../utils/resources";
import {Debug} from "../utils/debug";
import GUI from "lil-gui";

export class Board {
    private readonly scene: Scene;
    private readonly modelResource: ModelResource;
    private readonly model: Group;
    // Utils
    private readonly resources: Resources;
    // Debug
    private readonly debugFolder?: GUI;

    constructor(scene: Scene, resources: Resources, debug: Debug) {
        this.scene = scene;
        this.resources = resources;

        this.modelResource = resources.getModelResourceByName('resistanceGameBoard');
        this.model = this.modelResource.gltf.scene;
        this.configureModel();
        this.scene.add(this.model);

        // Debug
        if (debug.gui) {
            this.debugFolder = debug.gui.addFolder('board');
        }
    }


    private configureModel() {
        this.model.scale.set(0.2, 0.2, 0.2);
        this.model.traverse((child) => {
            if (child instanceof Mesh) {
                child.castShadow = true;
            }
        });
    }
}
