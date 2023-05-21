import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewstableComponent } from './newstable.component';

describe('NewstableComponent', () => {
  let component: NewstableComponent;
  let fixture: ComponentFixture<NewstableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewstableComponent]
    });
    fixture = TestBed.createComponent(NewstableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
