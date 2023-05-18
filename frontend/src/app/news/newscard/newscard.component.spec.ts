import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewscardComponent } from './newscard.component';

describe('NewscardComponent', () => {
  let component: NewscardComponent;
  let fixture: ComponentFixture<NewscardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewscardComponent]
    });
    fixture = TestBed.createComponent(NewscardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
