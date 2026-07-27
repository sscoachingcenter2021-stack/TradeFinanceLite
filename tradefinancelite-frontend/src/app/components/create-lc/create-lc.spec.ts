import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateLc } from './create-lc';

describe('CreateLc', () => {
  let component: CreateLc;
  let fixture: ComponentFixture<CreateLc>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateLc],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateLc);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
