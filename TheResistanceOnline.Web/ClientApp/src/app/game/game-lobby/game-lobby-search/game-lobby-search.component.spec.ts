import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameLobbySearchComponent } from './game-lobby-search.component';

describe('GameLobbySearchComponent', () => {
  let component: GameLobbySearchComponent;
  let fixture: ComponentFixture<GameLobbySearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GameLobbySearchComponent]
    });
    fixture = TestBed.createComponent(GameLobbySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
