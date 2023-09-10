import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameResistanceComponent } from './game-resistance.component';

describe('GameResistanceComponent', () => {
  let component: GameResistanceComponent;
  let fixture: ComponentFixture<GameResistanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameResistanceComponent]
    });
    fixture = TestBed.createComponent(GameResistanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
