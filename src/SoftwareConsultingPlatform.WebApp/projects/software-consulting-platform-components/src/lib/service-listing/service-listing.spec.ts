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
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render component', () => {
    fixture.detectChanges();
    const element = fixture.nativeElement.querySelector('.sc-service-listing');
    expect(element).toBeTruthy();
  });

  it('should display services', () => {
    component.services = [
      { title: 'Service 1', description: 'Description 1' },
      { title: 'Service 2', description: 'Description 2' },
    ];
    fixture.detectChanges();
    const items = fixture.nativeElement.querySelectorAll('.sc-service-item');
    expect(items.length).toBe(2);
  });

  it('should display service title and description', () => {
    component.services = [
      { title: 'Web Development', description: 'Build amazing websites' },
    ];
    fixture.detectChanges();
    const title = fixture.nativeElement.querySelector('.sc-service-item__title');
    const description = fixture.nativeElement.querySelector('.sc-service-item__description');
    expect(title?.textContent).toBe('Web Development');
    expect(description?.textContent).toBe('Build amazing websites');
  });

  it('should display service features', () => {
    component.services = [
      {
        title: 'Service',
        description: 'Description',
        features: ['Feature 1', 'Feature 2', 'Feature 3'],
      },
    ];
    fixture.detectChanges();
    const features = fixture.nativeElement.querySelectorAll('.sc-service-item__features li');
    expect(features.length).toBe(3);
    expect(features[0].textContent).toContain('Feature 1');
  });

  it('should display service icon', () => {
    component.services = [
      { icon: '🚀', title: 'Service', description: 'Description' },
    ];
    fixture.detectChanges();
    const icon = fixture.nativeElement.querySelector('.sc-service-item__icon');
    expect(icon?.textContent?.trim()).toBe('🚀');
  });

  it('should display service link', () => {
    component.services = [
      { title: 'Service', description: 'Description', link: '/service' },
    ];
    fixture.detectChanges();
    const link = fixture.nativeElement.querySelector('.sc-service-item__link') as HTMLAnchorElement;
    expect(link).toBeTruthy();
    expect(link.href).toContain('/service');
  });
});
