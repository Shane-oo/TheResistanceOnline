import * as lilGui from 'lil-gui';

export default class Debug {
  public active: boolean;
  public gui!: lilGui.GUI;

  constructor() {
    this.active = window.location.hash === '#debug';
    console.log(this.active);

    if(this.active) {
      this.gui = new lilGui.GUI();
      this.gui.domElement.id = 'gui';
    }
  }
}
