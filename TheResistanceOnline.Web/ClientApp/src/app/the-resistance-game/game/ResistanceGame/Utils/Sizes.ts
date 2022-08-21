import EventEmitter from './EventEmitter';

export default class Sizes extends EventEmitter {
  public width: number = 0;
  public height: number = 0;
  public pixelRatio: number = 0;

  constructor() {

    super();

    this.width = window.innerWidth / 1.5;
    this.height = window.innerHeight / 1.65;
    this.pixelRatio = Math.min(window.devicePixelRatio, 2);

    // Resize event
    window.addEventListener('resize', () => {
      this.width = window.innerWidth / 1.5;
      this.height = window.innerHeight / 1.65;
      this.pixelRatio = Math.min(window.devicePixelRatio, 2);

      this.trigger('resize');

    });

  }
}
