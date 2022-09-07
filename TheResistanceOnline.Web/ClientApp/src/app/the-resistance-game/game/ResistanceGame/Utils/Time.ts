import EventEmitter from './EventEmitter';

export default class Time extends EventEmitter {
  public start: number = 0;
  public current: number = 0;
  public elapsed: number = 0;
  public delta: number = 0;

  constructor() {
    super();

    // Setup
    this.start = Date.now();
    this.current = this.start;
    this.elapsed = 0;
    // magic number to stop bugs, 16fps
    this.delta = 16;

    window.requestAnimationFrame(() => {
      this.tick();
    });


  }

  private tick() {

    const currentTime = Date.now();
    this.delta = currentTime - this.current;
    this.current = currentTime;
    this.elapsed = this.current - this.start;

    this.trigger('tick');

    window.requestAnimationFrame(() => {
      this.tick();
    });
  }
}
