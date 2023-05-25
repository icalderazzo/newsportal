import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorycardComponent } from './storycard.component';

describe('StorycardComponent', () => {
  let component: StorycardComponent;
  let fixture: ComponentFixture<StorycardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StorycardComponent]
    });
    fixture = TestBed.createComponent(StorycardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
