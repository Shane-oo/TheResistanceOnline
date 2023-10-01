import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameResistanceClassicComponent } from './game-resistance-classic.component';

describe('GameResistanceClassicComponent', () => {
  let component: GameResistanceClassicComponent;
  let fixture: ComponentFixture<GameResistanceClassicComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameResistanceClassicComponent]
    });
    fixture = TestBed.createComponent(GameResistanceClassicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
