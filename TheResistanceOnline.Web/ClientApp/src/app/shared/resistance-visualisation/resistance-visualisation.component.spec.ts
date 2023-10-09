import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResistanceVisualisationComponent } from './resistance-visualisation.component';

describe('ResistanceVisualisationComponent', () => {
  let component: ResistanceVisualisationComponent;
  let fixture: ComponentFixture<ResistanceVisualisationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ResistanceVisualisationComponent]
    });
    fixture = TestBed.createComponent(ResistanceVisualisationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
