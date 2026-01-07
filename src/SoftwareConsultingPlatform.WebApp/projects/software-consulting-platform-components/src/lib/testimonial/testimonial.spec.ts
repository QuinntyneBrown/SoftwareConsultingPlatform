import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Testimonial } from './testimonial';

describe('Testimonial', () => {
  let component: Testimonial;
  let fixture: ComponentFixture<Testimonial>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Testimonial],
    }).compileComponents();

    fixture = TestBed.createComponent(Testimonial);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render component', () => {
    component.quote = 'Great service!';
    component.authorName = 'John Doe';
    fixture.detectChanges();
    const element = fixture.nativeElement.querySelector('.sc-testimonial');
    expect(element).toBeTruthy();
  });

  it('should display quote', () => {
    component.quote = 'Amazing experience!';
    component.authorName = 'Jane';
    fixture.detectChanges();
    const quote = fixture.nativeElement.querySelector('.sc-testimonial__quote');
    expect(quote?.textContent).toContain('Amazing experience!');
  });

  it('should display author name and position', () => {
    component.authorName = 'John Smith';
    component.authorPosition = 'CEO';
    component.authorCompany = 'TechCorp';
    component.quote = 'Great!';
    fixture.detectChanges();
    const name = fixture.nativeElement.querySelector('.sc-testimonial__author-name');
    const position = fixture.nativeElement.querySelector('.sc-testimonial__author-position');
    expect(name?.textContent).toBe('John Smith');
    expect(position?.textContent).toContain('CEO');
    expect(position?.textContent).toContain('TechCorp');
  });

  it('should display rating stars', () => {
    component.rating = 5;
    component.quote = 'Perfect!';
    component.authorName = 'Test';
    fixture.detectChanges();
    const stars = fixture.nativeElement.querySelectorAll('.sc-testimonial__star--filled');
    expect(stars.length).toBe(5);
  });

  it('should display avatar when provided', () => {
    component.avatarUrl = 'test.jpg';
    component.quote = 'Good';
    component.authorName = 'Test';
    fixture.detectChanges();
    const avatar = fixture.nativeElement.querySelector('.sc-testimonial__avatar') as HTMLImageElement;
    expect(avatar).toBeTruthy();
    expect(avatar.src).toContain('test.jpg');
  });

  it('should generate correct stars array', () => {
    component.rating = 4;
    expect(component.stars.length).toBe(4);
    expect(component.emptyStars.length).toBe(1);
  });

  it('should handle no rating', () => {
    component.rating = undefined;
    expect(component.stars.length).toBe(0);
    expect(component.emptyStars.length).toBe(0);
  });
});
