import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLobbyWaitingRoomDetailsComponent } from './game-lobby-waiting-room-details.component';

describe('GameLobbyWaitingRoomDetailsComponent', () => {
  let component: GameLobbyWaitingRoomDetailsComponent;
  let fixture: ComponentFixture<GameLobbyWaitingRoomDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameLobbyWaitingRoomDetailsComponent]
    });
    fixture = TestBed.createComponent(GameLobbyWaitingRoomDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
