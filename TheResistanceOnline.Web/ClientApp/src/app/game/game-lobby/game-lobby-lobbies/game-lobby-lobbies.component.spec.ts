import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLobbyLobbiesComponent } from './game-lobby-lobbies.component';

describe('GameLobbyLobbiesComponent', () => {
  let component: GameLobbyLobbiesComponent;
  let fixture: ComponentFixture<GameLobbyLobbiesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameLobbyLobbiesComponent]
    });
    fixture = TestBed.createComponent(GameLobbyLobbiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
