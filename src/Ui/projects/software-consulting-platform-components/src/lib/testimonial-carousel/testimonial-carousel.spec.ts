import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { TestimonialCarousel } from './testimonial-carousel';

describe('TestimonialCarousel', () => {
  let component: TestimonialCarousel;
  let fixture: ComponentFixture<TestimonialCarousel>;

  const mockTestimonials = [
    {
      quote: 'Great service!',
      authorName: 'John Doe',
      authorPosition: 'CEO',
      authorCompany: 'Tech Corp',
      rating: 5,
    },
    {
      quote: 'Excellent work!',
      authorName: 'Jane Smith',
      authorPosition: 'CTO',
      authorCompany: 'Innovation Inc',
      rating: 5,
    },
    {
      quote: 'Highly recommend!',
      authorName: 'Bob Johnson',
      authorPosition: 'Director',
      authorCompany: 'Global Ltd',
      rating: 4,
    },
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestimonialCarousel],
    }).compileComponents();

    fixture = TestBed.createComponent(TestimonialCarousel);
    component = fixture.componentInstance;
    component.testimonials = mockTestimonials;
    fixture.detectChanges();
  });

  afterEach(() => {
    component.ngOnDestroy();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should start at index 0', () => {
    expect(component.currentIndex).toBe(0);
  });

  it('should navigate to next testimonial', () => {
    component.next();
    expect(component.currentIndex).toBe(1);
  });

  it('should navigate to previous testimonial', () => {
    component.currentIndex = 1;
    component.previous();
    expect(component.currentIndex).toBe(0);
  });

  it('should wrap around when navigating past end', () => {
    component.currentIndex = 2;
    component.next();
    expect(component.currentIndex).toBe(0);
  });

  it('should wrap around when navigating before start', () => {
    component.currentIndex = 0;
    component.previous();
    expect(component.currentIndex).toBe(2);
  });

  it('should go to specific slide', () => {
    component.goToSlide(2);
    expect(component.currentIndex).toBe(2);
  });

  it('should render section title when provided', () => {
    component.sectionTitle = 'What Our Clients Say';
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.sc-testimonial-carousel__title')?.textContent).toContain('What Our Clients Say');
  });

  it('should render navigation dots', () => {
    const compiled = fixture.nativeElement;
    const dots = compiled.querySelectorAll('.sc-testimonial-carousel__dot');
    expect(dots.length).toBe(3);
  });
});
