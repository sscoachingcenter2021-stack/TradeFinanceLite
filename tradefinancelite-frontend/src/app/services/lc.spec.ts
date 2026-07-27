import { TestBed } from '@angular/core/testing';

import { Lc } from './lc';

describe('Lc', () => {
  let service: Lc;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Lc);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
