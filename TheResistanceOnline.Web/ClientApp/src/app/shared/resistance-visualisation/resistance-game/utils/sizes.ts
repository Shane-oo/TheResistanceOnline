import {Subject} from "rxjs";

export class Sizes {
  resizeSubject: Subject<void> = new Subject<void>();

  constructor() {
    // Setup
    this._width = window.innerWidth;
    this._height = window.innerHeight
    this._pixelRatio = Math.min(window.devicePixelRatio, 2);

    // Resize event
    window.addEventListener('resize', this.resize);
  }

  private _height: number;

  get height(): number {
    return this._height;
  }

  private _pixelRatio: number;

  get pixelRatio(): number {
    return this._pixelRatio;
  }

  private _width: number;

  get width(): number {
    return this._width;
  }

  destroy() {
    window.removeEventListener('resize', this.resize);
  }

  private resize = () => {
    this._width = window.innerWidth;
    this._height = window.innerHeight
    this._pixelRatio = Math.min(window.devicePixelRatio, 2);

    this.resizeSubject.next();
  }
}
