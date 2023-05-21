import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewssearchbarComponent } from './newssearchbar.component';

describe('NewssearchbarComponent', () => {
  let component: NewssearchbarComponent;
  let fixture: ComponentFixture<NewssearchbarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewssearchbarComponent]
    });
    fixture = TestBed.createComponent(NewssearchbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
