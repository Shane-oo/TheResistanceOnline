import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLobbyWaitingRoomComponent } from './game-lobby-waiting-room.component';

describe('GameLobbyWaitingRoomComponent', () => {
  let component: GameLobbyWaitingRoomComponent;
  let fixture: ComponentFixture<GameLobbyWaitingRoomComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameLobbyWaitingRoomComponent]
    });
    fixture = TestBed.createComponent(GameLobbyWaitingRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
