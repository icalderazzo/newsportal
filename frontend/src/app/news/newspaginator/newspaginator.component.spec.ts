import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewspaginatorComponent } from './newspaginator.component';

describe('NewspaginatorComponent', () => {
  let component: NewspaginatorComponent;
  let fixture: ComponentFixture<NewspaginatorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewspaginatorComponent]
    });
    fixture = TestBed.createComponent(NewspaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
