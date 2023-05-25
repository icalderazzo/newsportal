import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoriespaginatorComponent } from './storiespaginator.component';

describe('NewspaginatorComponent', () => {
  let component: StoriespaginatorComponent;
  let fixture: ComponentFixture<StoriespaginatorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StoriespaginatorComponent]
    });
    fixture = TestBed.createComponent(StoriespaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
