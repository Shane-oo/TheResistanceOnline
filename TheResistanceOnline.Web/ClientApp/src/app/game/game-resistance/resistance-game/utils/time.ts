import {Subject} from "rxjs";


export class Time {
  tickSubject: Subject<void> = new Subject<void>();
  private readonly start: number;
  private current: number;
  private _elapsed: number;
  private destroyed: boolean = false;
  private animationFrameRequestId: number = 0;

  constructor() {
    this.start = Date.now();
    this.current = this.start;
    this._elapsed = 0;
    this._delta = 16; // 16 fps default

    // wait one frame before starting tick
    window.requestAnimationFrame(() => {
      this.tick();
    });
  }

  private _delta: number;

  get delta(): number {
    return this._delta;
  }

  destroy() {
    this.destroyed = true;
  }

  private tick() {
    if (this.destroyed) {
      window.cancelAnimationFrame(this.animationFrameRequestId);
      return;
    }

    const currentTime = Date.now();
    this._delta = currentTime - this.current;
    this.current = currentTime;
    this._elapsed = this.current - this.start;

    this.tickSubject.next();

    this.animationFrameRequestId = window.requestAnimationFrame(() => {
      this.tick();
    });
  }
}
