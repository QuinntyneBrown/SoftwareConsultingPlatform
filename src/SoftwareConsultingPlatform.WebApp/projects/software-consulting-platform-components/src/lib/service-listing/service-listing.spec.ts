import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServiceListing } from './service-listing';

describe('ServiceListing', () => {
  let component: ServiceListing;
  let fixture: ComponentFixture<ServiceListing>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceListing],
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceListing);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render component', () => {
    fixture.detectChanges();
    const element = fixture.nativeElement.querySelector('.sc-service-listing');
    expect(element).toBeTruthy();
  });
});
