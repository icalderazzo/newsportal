import { TestBed } from '@angular/core/testing';

import { NewsportalService } from './newsportal.service';

describe('NewsportalService', () => {
  let service: NewsportalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewsportalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
