import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDetailsFriendsComponent } from './user-details-friends.component';

describe('UserDetailsFriendsComponent', () => {
  let component: UserDetailsFriendsComponent;
  let fixture: ComponentFixture<UserDetailsFriendsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserDetailsFriendsComponent]
    });
    fixture = TestBed.createComponent(UserDetailsFriendsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
