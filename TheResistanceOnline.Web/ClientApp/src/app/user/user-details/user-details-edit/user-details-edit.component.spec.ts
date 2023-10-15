import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDetailsEditComponent } from './user-details-edit.component';

describe('UserDetailsEditComponent', () => {
  let component: UserDetailsEditComponent;
  let fixture: ComponentFixture<UserDetailsEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserDetailsEditComponent]
    });
    fixture = TestBed.createComponent(UserDetailsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
