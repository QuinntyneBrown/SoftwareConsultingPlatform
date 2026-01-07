import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SoftwareConsultingPlatformComponents } from './software-consulting-platform-components';

describe('SoftwareConsultingPlatformComponents', () => {
  let component: SoftwareConsultingPlatformComponents;
  let fixture: ComponentFixture<SoftwareConsultingPlatformComponents>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SoftwareConsultingPlatformComponents]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SoftwareConsultingPlatformComponents);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
