import {AnimationAction, AnimationMixer, Group, Mesh, Scene} from "three";
import {ModelResource, Resources} from "../utils/resources";
import {Time} from "../utils/time";
import {Debug} from "../utils/debug";
import GUI from "lil-gui";
import {Dispose} from "../utils/dispose";

export class Fox {
    private readonly scene: Scene;
    private readonly modelResource: ModelResource;
    private readonly model: Group;
    private readonly animationMixer: AnimationMixer;
    private actions!: {
        current?: AnimationAction,
        idle: AnimationAction,
        walking: AnimationAction,
        running: AnimationAction,

    }
    // Utils
    private readonly resources: Resources;
    private readonly time: Time;

    // Debug
    private readonly debug: Debug;
    private readonly debugFolder?: GUI;

    constructor(scene: Scene, time: Time, resources: Resources, debug: Debug) {
        this.scene = scene;
        this.time = time;
        this.resources = resources;
        this.debug = debug;

        this.modelResource = resources.getModelResourceByName('testGltfModel');
        this.model = this.modelResource.gltf.scene;
        this.configureModel();
        this.scene.add(this.model);

        this.animationMixer = new AnimationMixer(this.model);

        // Debug
        if (this.debug.gui) {
            this.debugFolder = this.debug.gui.addFolder('fox');
        }

        this.setAnimations();
    }

    update() {
        this.animationMixer.update(this.time.delta / 1000);
    }

    destroy() {
        this.scene.remove(this.model);
        this.model.traverse((child) => {
            Dispose.disposeOfChild(child);
        });
        this.animationMixer.stopAllAction();
        this.animationMixer.uncacheRoot(this.model);
        this.debugFolder?.destroy();
    }

    private configureModel() {
        this.model.scale.set(0.02, 0.02, 0.02);
        this.model.traverse((child) => {
            if (child instanceof Mesh) {
                child.castShadow = true;
            }
        });
    }

    private setAnimations() {
        this.actions = {

            idle: this.animationMixer.clipAction(this.modelResource.gltf.animations[0]),
            walking: this.animationMixer.clipAction(this.modelResource.gltf.animations[1]),
            running: this.animationMixer.clipAction(this.modelResource.gltf.animations[2]),

        };

        if (this.debug.gui) {
            const debugObject = {
                playIdle: () => {
                    this.playAction(this.actions.idle);
                },
                playRunning: () => {
                    this.playAction(this.actions.running);
                },
                playWalking: () => {
                    this.playAction(this.actions.walking);
                }
            };
            this.debugFolder?.add(debugObject, 'playIdle');
            this.debugFolder?.add(debugObject, 'playWalking');
            this.debugFolder?.add(debugObject, 'playRunning');
        }
    }

    private playAction = (newAction: AnimationAction) => {
        const oldAction = this.actions.current;

        newAction.reset();
        newAction.play();
        if (oldAction) {
            newAction.crossFadeFrom(oldAction, 1, false);
        }
        this.actions.current = newAction;
    }
}
