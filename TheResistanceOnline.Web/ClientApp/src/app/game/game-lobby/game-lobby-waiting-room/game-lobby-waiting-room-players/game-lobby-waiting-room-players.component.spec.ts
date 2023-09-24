import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLobbyWaitingRoomPlayersComponent } from './game-lobby-waiting-room-players.component';

describe('GameLobbyWaitingRoomPlayersComponent', () => {
  let component: GameLobbyWaitingRoomPlayersComponent;
  let fixture: ComponentFixture<GameLobbyWaitingRoomPlayersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameLobbyWaitingRoomPlayersComponent]
    });
    fixture = TestBed.createComponent(GameLobbyWaitingRoomPlayersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
