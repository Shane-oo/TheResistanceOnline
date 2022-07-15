import { AfterViewInit, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import * as lilGui from 'lil-gui';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { Object3D } from 'three';

@Component({
             selector: 'app-game-canvas',
             templateUrl: './game-canvas.component.html',
             styleUrls: ['./game-canvas.component.css']
           })
export class GameCanvasComponent implements OnInit, AfterViewInit {
  private loadingManager: THREE.LoadingManager = new THREE.LoadingManager();
  private textureLoader: THREE.TextureLoader = new THREE.TextureLoader(this.loadingManager);
  private cubeTextureLoader: THREE.CubeTextureLoader = new THREE.CubeTextureLoader();
  private environmentMap: THREE.CubeTexture = new THREE.CubeTexture();
  private gltfLoader: GLTFLoader = new GLTFLoader();
  @ViewChild('canvas')
  private canvasRef!: ElementRef;

  private get canvas(): HTMLCanvasElement {
    return this.canvasRef.nativeElement;
  }

  private renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer();
  private scene: THREE.Scene = new THREE.Scene();
  private camera: THREE.PerspectiveCamera = new THREE.PerspectiveCamera;
  private controls!: OrbitControls;
  private width = window.innerWidth / 1.5;
  private height = window.innerHeight / 1.65;


  private directionalLight: THREE.DirectionalLight = new THREE.DirectionalLight();


  //Initialisations
  private initCamera() {
    this.camera = new THREE.PerspectiveCamera(75, this.width / this.height, 0.1, 100);
    this.camera.position.set(4, 1, -4);
  }

  private initControls() {
    this.controls = new OrbitControls(this.camera, this.canvas);
    this.controls.enableDamping = true;
  }

  private initRenderer() {
    this.renderer = new THREE.WebGLRenderer({canvas: this.canvas, antialias: true});
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    this.renderer.setSize(this.width, this.height);
    this.renderer.physicallyCorrectLights = true;
    this.renderer.outputEncoding = THREE.sRGBEncoding;
    this.renderer.toneMapping = THREE.ReinhardToneMapping;
    this.renderer.toneMappingExposure = 3;

    this.renderer.shadowMap.enabled = true;
    this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
  }

  private initScene() {
    this.scene.background = new THREE.Color('black');
  }

  private initLights() {
    this.directionalLight = new THREE.DirectionalLight('#ffffff', 3);
    this.directionalLight.position.set(0.25, 3, -2.25);
    this.directionalLight.castShadow = true;
    this.directionalLight.shadow.camera.far = 15;
    this.directionalLight.shadow.mapSize.set(1024, 1024);
    this.directionalLight.shadow.normalBias = 0.02;
  }

  private initAssets() {
    this.gltfLoader.load('../../../assets/models/test-models/burger.glb', (gltf) => {
      gltf.scene.scale.set(0.3, 0.3, 0.3);
      gltf.scene.position.set(0, -1, 0);
      gltf.scene.rotation.y = Math.PI * 0.5;
      this.scene.add(gltf.scene);

      this.updateAllMaterials();
    });

    this.environmentMap = this.cubeTextureLoader.load([
                                                        '../../../assets/environment-maps/0/px.jpg',
                                                        '../../../assets/environment-maps/0/nx.jpg',
                                                        '../../../assets/environment-maps/0/py.jpg',
                                                        '../../../assets/environment-maps/0/ny.jpg',
                                                        '../../../assets/environment-maps/0/pz.jpg',
                                                        '../../../assets/environment-maps/0/nz.jpg'
                                                      ]);
    this.environmentMap.encoding = THREE.sRGBEncoding;
    this.modifyScene(this.environmentMap);

  }

  private initDebugGui() {

  }

  // Modifications
  private modifyScene(environmentMap?: THREE.CubeTexture) {
    if(environmentMap) {
      this.scene.background = environmentMap;
      this.scene.environment = environmentMap;
    }
  }

  private modifyCamera() {
  }

  private modifyControls() {

  }

  private modifyLights() {

  }

  private modifyMaterials() {

  }

  private updateAllMaterials() {
    this.scene.traverse((child: Object3D) => {

      if(child instanceof THREE.Mesh && child.material instanceof THREE.MeshStandardMaterial) {
        child.material.envMap = this.environmentMap;
        child.material.envMapIntensity = 2.5;
        child.castShadow = true;
        child.receiveShadow = true;
      }
    });
  }

  private addObjectsToScene() {
    this.scene.add(this.camera);
    this.scene.add(this.directionalLight);

  }

  // Create Scene
  private createScene() {
    this.initScene();
    this.initCamera();
    this.initControls();


    this.initLights();
    this.initAssets();
    this.initDebugGui();
    // Add Objects to the scene
    this.addObjectsToScene();
  }

  // Animation
  public clock = new THREE.Clock();

  private animateObjects() {
    // Time
    const elapsedTime = this.clock.getElapsedTime();
    // Update Controls
    this.controls.update();
  }

  private startRenderingLoop() {
    // Renderer
    this.initRenderer();
    // Activate shadow map

    let component: GameCanvasComponent = this;
    (function render() {
      requestAnimationFrame(render);
      // Call animation Functions
      component.animateObjects();
      // render renderer
      component.renderer.render(component.scene, component.camera);
    }());
  }

  constructor() {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    // call needed functions
    this.createScene();
    this.startRenderingLoop();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Window) {
    this.width = window.innerWidth / 1.5;
    this.height = window.innerHeight / 1.65;
    // Update Camera
    this.camera.aspect = this.width / this.height;
    // Alert Camera needs to update projection Matrix
    this.camera.updateProjectionMatrix();
    // Update render
    this.renderer.setSize(this.width, this.height);
    // In-case pixel ratio changes when moving screens
    this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
  }
}
