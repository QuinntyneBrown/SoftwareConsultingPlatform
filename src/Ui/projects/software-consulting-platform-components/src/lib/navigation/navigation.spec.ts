import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Navigation } from './navigation';

describe('Navigation', () => {
  let component: Navigation;
  let fixture: ComponentFixture<Navigation>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Navigation],
    }).compileComponents();

    fixture = TestBed.createComponent(Navigation);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render component', () => {
    fixture.detectChanges();
    const element = fixture.nativeElement.querySelector('.sc-navigation');
    expect(element).toBeTruthy();
  });

  it('should display logo', () => {
    component.logoSrc = 'logo.png';
    component.logoAlt = 'Company Logo';
    fixture.detectChanges();
    const logo = fixture.nativeElement.querySelector('.sc-navigation__logo img') as HTMLImageElement;
    expect(logo).toBeTruthy();
    expect(logo.src).toContain('logo.png');
  });

  it('should display links', () => {
    component.links = [
      { label: 'Home', routerLink: '/' },
      { label: 'About', routerLink: '/about' },
    ];
    fixture.detectChanges();
    const links = fixture.nativeElement.querySelectorAll('.sc-navigation__link');
    expect(links.length).toBe(2);
    expect(links[0].textContent?.trim()).toBe('Home');
    expect(links[1].textContent?.trim()).toBe('About');
  });

  it('should toggle mobile menu', () => {
    expect(component.mobileMenuOpen).toBe(false);
    component.toggleMobileMenu();
    expect(component.mobileMenuOpen).toBe(true);
    component.toggleMobileMenu();
    expect(component.mobileMenuOpen).toBe(false);
  });

  it('should apply sticky class when sticky is true', () => {
    component.sticky = true;
    fixture.detectChanges();
    const nav = fixture.nativeElement.querySelector('.sc-navigation');
    expect(nav.classList.contains('sc-navigation--sticky')).toBe(true);
  });
});
