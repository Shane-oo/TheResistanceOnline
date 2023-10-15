import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameStreamVideoComponent } from './game-stream-video.component';

describe('GameStreamVideoComponent', () => {
  let component: GameStreamVideoComponent;
  let fixture: ComponentFixture<GameStreamVideoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameStreamVideoComponent]
    });
    fixture = TestBed.createComponent(GameStreamVideoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
