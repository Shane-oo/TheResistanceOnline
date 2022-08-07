import ResistanceGame from '../ResistanceGame';
import Environment from './Environment';
import * as THREE from 'three';
import Resources from '../Utils/Resources';
import Floor from './Floor';
import Fox from './Fox';


export default class World {
  public resistanceGame: ResistanceGame;
  public scene: THREE.Scene;
  public environment!: Environment;
  public resources: Resources;
  public floor!: Floor;
  public fox!: Fox;

  constructor() {
    this.resistanceGame = new ResistanceGame();
    this.scene = this.resistanceGame.scene;
    this.resources = this.resistanceGame.resources;

    // Wait for resources
    this.resources.on('ready', () => {
      this.floor = new Floor();
      this.fox = new Fox();
      this.environment = new Environment();

    });

  }

  public update() {
    if(this.fox) {
      this.fox.update();
    }
  }

}
