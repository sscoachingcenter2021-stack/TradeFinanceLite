import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkBackground } from './network-background';

describe('NetworkBackground', () => {
  let component: NetworkBackground;
  let fixture: ComponentFixture<NetworkBackground>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NetworkBackground],
    }).compileComponents();

    fixture = TestBed.createComponent(NetworkBackground);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
