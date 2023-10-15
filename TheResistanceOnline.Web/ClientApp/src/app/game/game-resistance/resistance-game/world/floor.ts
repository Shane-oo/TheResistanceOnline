import {CircleGeometry, Mesh, MeshStandardMaterial, RepeatWrapping, Scene, SRGBColorSpace} from "three";
import {Resources, TextureResource} from "../utils/resources";

export class Floor {
    private readonly scene: Scene;
    private readonly resources: Resources;
    private textures!: {
        diffuse: TextureResource,
        normal: TextureResource
    };
    private readonly mesh: Mesh<CircleGeometry, MeshStandardMaterial>;

    constructor(scene: Scene, resources: Resources) {
        this.scene = scene;
        this.resources = resources;

        this.setTextures();

        const geometry = new CircleGeometry(5, 64);
        const material = new MeshStandardMaterial({
            map: this.textures.diffuse.texture,
            normalMap: this.textures.normal.texture
        });

        this.mesh = new Mesh<CircleGeometry, MeshStandardMaterial>(geometry, material);
        this.configureMesh();
        this.scene.add(this.mesh);
    }

    destroy() {
        this.scene.remove(this.mesh);
        this.mesh.geometry.dispose();
        this.mesh.material.dispose();
        this.textures.normal.texture.dispose();
        this.textures.diffuse.texture.dispose();
    }

    private setTextures() {
        const diffuse = this.resources.getTextureResourceByName('floorDiffuseTexture');
        diffuse.texture.colorSpace = SRGBColorSpace;
        diffuse.texture.repeat.set(1.5, 1.5);
        diffuse.texture.wrapS = diffuse.texture.wrapT = RepeatWrapping;

        const normal = this.resources.getTextureResourceByName('floorNormalTexture');
        normal.texture.repeat.set(1.5, 1.5);
        normal.texture.wrapS = normal.texture.wrapT = RepeatWrapping;

        this.textures = {
            diffuse: diffuse,
            normal: normal
        };
    }

    private configureMesh() {
        this.mesh.rotation.x = -Math.PI * 0.5;
        this.mesh.receiveShadow = true;
    }
}
