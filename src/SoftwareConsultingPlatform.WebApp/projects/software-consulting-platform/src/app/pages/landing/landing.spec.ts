import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Landing } from './landing';
import { LandingService } from '../../services/landing-service';
import { of } from 'rxjs';

describe('Landing', () => {
  let component: Landing;
  let fixture: ComponentFixture<Landing>;
  let mockLandingService: jest.Mocked<LandingService>;

  const mockData = {
    navLinks: [{ label: 'Home', routerLink: '/' }],
    heroHeadline: 'Test Headline',
    heroSubheadline: 'Test Subheadline',
    services: [],
    caseStudies: [],
    valuePropositions: [],
    testimonials: [],
    footerColumns: [],
    footerTagline: 'Test Tagline',
    copyright: '© 2026',
  };

  beforeEach(async () => {
    mockLandingService = {
      getLandingPageData: jest.fn().mockReturnValue(of(mockData)),
    } as unknown as jest.Mocked<LandingService>;

    await TestBed.configureTestingModule({
      imports: [Landing],
      providers: [{ provide: LandingService, useValue: mockLandingService }],
    }).compileComponents();

    fixture = TestBed.createComponent(Landing);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call landing service on init', () => {
    expect(mockLandingService.getLandingPageData).toHaveBeenCalled();
  });
});
