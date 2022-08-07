import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import ResistanceGame from '../ResistanceGame/ResistanceGame';

@Component({
             selector: 'app-game-canvas',
             templateUrl: './game-canvas.component.html',
             styleUrls: ['./game-canvas.component.css']
           })
export class GameCanvasComponent implements OnInit, AfterViewInit {
  @ViewChild('canvas')
  private canvasRef!: ElementRef;
  private resistanceGame!: ResistanceGame;//= new ResistanceGame(this.canvas);

  constructor() {
  }

  private get canvas(): HTMLCanvasElement {
    return this.canvasRef.nativeElement;
  }

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    this.resistanceGame = new ResistanceGame(this.canvas);
    // // some stupid thing needed for angular to work
    // let resistanceGame =  this.resistanceGame;
    // (function render() {
    //   requestAnimationFrame(render);
    //   // Call animation Functions
    //   // render renderer
    //   resistanceGame.renderer.instance.render(resistanceGame.scene, resistanceGame.camera.instance);
    // }());
  }

  //, AfterViewInit {
  // private loadingManager: THREE.LoadingManager = new THREE.LoadingManager();
  // private textureLoader: THREE.TextureLoader = new THREE.TextureLoader(this.loadingManager);
  // private cubeTextureLoader: THREE.CubeTextureLoader = new THREE.CubeTextureLoader();
  // private environmentMap: THREE.CubeTexture = new THREE.CubeTexture();
  // private gltfLoader: GLTFLoader = new GLTFLoader();
  // private foxMixer!: THREE.AnimationMixer;

  //
  // private renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer();
  // private scene: THREE.Scene = new THREE.Scene();
  // private camera: THREE.PerspectiveCamera = new THREE.PerspectiveCamera;
  // private controls!: OrbitControls;
  // private width = window.innerWidth / 1.5;
  // private height = window.innerHeight / 1.65;
  //
  //
  // private directionalLight: THREE.DirectionalLight = new THREE.DirectionalLight();
  //
  // //Objects
  // private floorColorTexture = this.textureLoader.load('assets/textures/dirt/color.jpg');
  // private floorNormalTexture = this.textureLoader.load('assets/textures/dirt/normal.jpg');
  // private floorGeometry: THREE.CircleGeometry = new THREE.CircleGeometry(5, 64);
  // private floorMaterial: THREE.MeshStandardMaterial = new THREE.MeshStandardMaterial({
  //                                                                                      map: this.floorColorTexture,
  //                                                                                      normalMap: this.floorNormalTexture
  //                                                                                    });
  // private floor: THREE.Mesh = new THREE.Mesh(this.floorGeometry, this.floorMaterial);
  //
  //
  // //Initialisations
  // private initCamera() {
  //   this.camera = new THREE.PerspectiveCamera(75, this.width / this.height, 0.1, 100);
  //   this.camera.position.set(4, 1, -4);
  // }
  //
  // private initControls() {
  //   this.controls = new OrbitControls(this.camera, this.canvas);
  //   this.controls.enableDamping = true;
  // }
  //
  // private initRenderer() {
  //   this.renderer = new THREE.WebGLRenderer({canvas: this.canvas, antialias: true});
  //   this.renderer.physicallyCorrectLights = true;
  //   this.renderer.outputEncoding = THREE.sRGBEncoding;
  //   this.renderer.toneMapping = THREE.CineonToneMapping;
  //   this.renderer.toneMappingExposure = 1.75;
  //   this.renderer.shadowMap.enabled = true;
  //   this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
  //   this.renderer.setClearColor('#211d20');
  //   this.renderer.setSize(this.width, this.height);
  //   this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
  // }
  //
  // private initScene() {
  //   this.scene.background = new THREE.Color('black');
  // }
  //
  // private initLights() {
  //   this.directionalLight = new THREE.DirectionalLight('#ffffff', 3);
  //   this.directionalLight.position.set(3.5, 2, -1.25);
  //   this.directionalLight.castShadow = true;
  //   this.directionalLight.shadow.camera.far = 15;
  //   this.directionalLight.shadow.mapSize.set(1024, 1024);
  //   this.directionalLight.shadow.normalBias = 0.05;
  // }
  //
  // private initFloor() {
  //
  // }
  //
  // private initAssets() {
  //   this.floorGeometry = new THREE.CircleGeometry(5, 64);
  //   this.floorMaterial = new THREE.MeshStandardMaterial({
  //                                                         map: this.floorColorTexture,
  //                                                         normalMap: this.floorNormalTexture
  //                                                       });
  //   this.floor = new THREE.Mesh(this.floorGeometry, this.floorMaterial);
  //
  //   this.gltfLoader.load('assets/models/Fox/glTF/Fox.gltf', (gltf) => {
  //     // Model
  //     gltf.scene.scale.set(0.02, 0.02, 0.02);
  //     this.scene.add(gltf.scene);
  //
  //     // Animation
  //     this.foxMixer = new THREE.AnimationMixer(gltf.scene);
  //     let foxAction = this.foxMixer.clipAction(gltf.animations[0]);
  //     foxAction.play();
  //
  //     this.updateAllMaterials();
  //   });
  //
  //
  //
  //   // floor  texture
  //   this.floorColorTexture.encoding = THREE.sRGBEncoding;
  //   this.floorColorTexture.repeat.set(1.5, 1.5);
  //   this.floorColorTexture.wrapS = THREE.RepeatWrapping;
  //   this.floorColorTexture.wrapT = THREE.RepeatWrapping;
  //
  //   // floor normal
  //   this.floorNormalTexture.repeat.set(1.5, 1.5);
  //   this.floorNormalTexture.wrapS = THREE.RepeatWrapping;
  //   this.floorNormalTexture.wrapT = THREE.RepeatWrapping;
  //
  //   // floor
  //   this.floor.rotation.x = -Math.PI * 0.5;
  // }
  //
  // private initDebugGui() {
  //
  // }
  //
  // // Modifications
  // private modifyScene(environmentMap?: THREE.CubeTexture) {
  //   if(environmentMap) {
  //     this.scene.background = environmentMap;
  //     this.scene.environment = environmentMap;
  //   }
  // }
  //
  // private modifyCamera() {
  // }
  //
  // private modifyControls() {
  //
  // }
  //
  // private modifyLights() {
  //
  // }
  //
  // private modifyMaterials() {
  //
  // }
  //
  // private updateAllMaterials() {
  //   this.scene.traverse((child: Object3D) => {
  //
  //     if(child instanceof THREE.Mesh && child.material instanceof THREE.MeshStandardMaterial) {
  //       child.material.needsUpdate = true;
  //       child.material.envMap = this.environmentMap;
  //       child.material.envMapIntensity = 2.5;
  //       child.castShadow = true;
  //       child.receiveShadow = true;
  //     }
  //   });
  // }
  //
  // private addObjectsToScene() {
  //   this.scene.add(this.camera);
  //   this.scene.add(this.directionalLight);
  //   this.scene.add(this.floor);
  // }
  //
  // // Create Scene
  // private createScene() {
  //
  //   this.initScene();
  //   this.initCamera();
  //   this.initControls();
  //
  //
  //   this.initLights();
  //   this.initAssets();
  //   this.initDebugGui();
  //   // Add Objects to the scene
  //   this.addObjectsToScene();
  // }
  //
  // // Animation
  // public clock = new THREE.Clock();
  // public previousTime = 0;
  //
  // private animateObjects() {
  //   // Time
  //   const elapsedTime = this.clock.getElapsedTime();
  //   const deltaTime = elapsedTime - this.previousTime;
  //   this.previousTime = elapsedTime;
  //   // Update Controls
  //   this.controls.update();
  //
  //   // Fox animation
  //   if(this.foxMixer) {
  //     this.foxMixer.update(deltaTime);
  //   }
  // }
  //
  // private startRenderingLoop() {
  //   // Renderer
  //   this.initRenderer();
  //   // Activate shadow map
  //
  //   let component: GameCanvasComponent = this;
  //   (function render() {
  //     requestAnimationFrame(render);
  //     // Call animation Functions
  //     component.animateObjects();
  //     // render renderer
  //     component.renderer.render(component.scene, component.camera);
  //   }());
  // }
  //
  // constructor() {
  // }
  //


  //
  // ngAfterViewInit() {
  //   // call needed functions
  //   this.createScene();
  //   this.startRenderingLoop();
  // }
  //
  // @HostListener('window:resize', ['$event'])
  // onResize(event: Window) {
  //   this.width = window.innerWidth / 1.5;
  //   this.height = window.innerHeight / 1.65;
  //   // Update Camera
  //   this.camera.aspect = this.width / this.height;
  //   // Alert Camera needs to update projection Matrix
  //   this.camera.updateProjectionMatrix();
  //   // Update render
  //   this.renderer.setSize(this.width, this.height);
  //   // In-case pixel ratio changes when moving screens
  //   this.renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
  // }
}
