import EventEmitter from './EventEmitter';

export default class Sizes extends EventEmitter {
  public width: number = 0;
  public height: number = 0;
  public pixelRatio: number = 0;

  constructor() {

    super();

    this.width = window.innerWidth / 1.75 ;   // magic number to stop if from extending width outside canvas div
    this.height = 325;// window.innerHeight / 2.28;
    console.log(this.height)
    this.pixelRatio = Math.min(window.devicePixelRatio, 2);

    // Resize event
    window.addEventListener('resize', () => {
      this.width = window.innerWidth / 1.75 ;
      this.height = 325;
      this.pixelRatio = Math.min(window.devicePixelRatio, 2);

      this.trigger('resize');

    });

  }
}
