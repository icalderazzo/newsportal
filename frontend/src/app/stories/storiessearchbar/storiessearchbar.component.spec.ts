import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoriessearchbarComponent } from './storiessearchbar.component';

describe('NewssearchbarComponent', () => {
  let component: StoriessearchbarComponent;
  let fixture: ComponentFixture<StoriessearchbarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StoriessearchbarComponent]
    });
    fixture = TestBed.createComponent(StoriessearchbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
