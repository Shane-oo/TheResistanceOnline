import {environment} from "../../../../../environments/environment";
import GUI from "lil-gui";

export class Debug {
    private readonly _gui?: GUI;

    constructor() {

        if (!environment.production) {
            this._gui = new GUI();
            this._gui.close();
        }
    }

    get gui(): GUI | undefined {
        return this._gui;
    }

    destroy() {
        this._gui?.destroy();
    }
}
