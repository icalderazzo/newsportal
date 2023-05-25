import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoriestableComponent } from './storiestable.component';

describe('StoriestableComponent', () => {
  let component: StoriestableComponent;
  let fixture: ComponentFixture<StoriestableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StoriestableComponent]
    });
    fixture = TestBed.createComponent(StoriestableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
