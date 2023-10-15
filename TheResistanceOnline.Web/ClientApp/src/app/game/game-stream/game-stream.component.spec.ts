import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameStreamComponent } from './game-stream.component';

describe('GameStreamComponent', () => {
  let component: GameStreamComponent;
  let fixture: ComponentFixture<GameStreamComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameStreamComponent]
    });
    fixture = TestBed.createComponent(GameStreamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
