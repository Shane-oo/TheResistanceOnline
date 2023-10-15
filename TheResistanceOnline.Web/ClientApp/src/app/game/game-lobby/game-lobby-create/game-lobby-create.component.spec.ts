import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLobbyCreateComponent } from './game-lobby-create.component';

describe('GameLobbyCreateComponent', () => {
  let component: GameLobbyCreateComponent;
  let fixture: ComponentFixture<GameLobbyCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameLobbyCreateComponent]
    });
    fixture = TestBed.createComponent(GameLobbyCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
