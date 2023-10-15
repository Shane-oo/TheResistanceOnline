// noinspection SuspiciousTypeOfGuard

import {Light, Mesh, Object3D} from "three";

export class Dispose {

    static disposeOfChild(child: Object3D) {
        if (child instanceof Mesh) {
            child.geometry.dispose();
            // Loop through the material properties
            for (const key in child.material) {
                const value = child.material[key]

                // Test if there is a dispose function
                if (value && typeof value.dispose === 'function') {

                    value.dispose()
                }
            }
        } else if (child instanceof Light) {
            child.dispose();
        }
    }

}
