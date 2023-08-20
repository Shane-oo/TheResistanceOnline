import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDetailsInteractionsComponent } from './user-details-interactions.component';

describe('UserDetailsInteractionsComponent', () => {
  let component: UserDetailsInteractionsComponent;
  let fixture: ComponentFixture<UserDetailsInteractionsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserDetailsInteractionsComponent]
    });
    fixture = TestBed.createComponent(UserDetailsInteractionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
