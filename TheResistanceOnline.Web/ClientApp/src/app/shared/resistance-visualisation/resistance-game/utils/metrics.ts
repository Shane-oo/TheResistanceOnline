import {environment} from "../../../../../environments/environment";
import Stats from "three/examples/jsm/libs/stats.module";

export class Metrics {

    private readonly stats?: Stats;

    constructor() {
        if (!environment.production) {
            this.stats = new Stats();

            this.stats.showPanel(0);
            document.body.appendChild(this.stats.dom);
        }
    }

    destroy() {
        this.stats?.dom.remove();
    }

    update() {
        this.stats?.update();
    }
}
