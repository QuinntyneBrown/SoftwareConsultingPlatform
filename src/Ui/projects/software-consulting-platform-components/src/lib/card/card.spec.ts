import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Card } from './card';

describe('Card', () => {
  let component: Card;
  let fixture: ComponentFixture<Card>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Card],
    }).compileComponents();

    fixture = TestBed.createComponent(Card);
    component = fixture.componentInstance;
    compiled = fixture.nativeElement;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display title', () => {
    component.title = 'Test Card';
    fixture.detectChanges();
    const title = compiled.querySelector('.sc-card__title');
    expect(title?.textContent).toBe('Test Card');
  });

  it('should display image when imageUrl is provided', () => {
    component.imageUrl = 'test.jpg';
    component.imageAlt = 'Test image';
    component.title = 'Test';
    fixture.detectChanges();
    const img = compiled.querySelector('.sc-card__image') as HTMLImageElement;
    expect(img).toBeTruthy();
    expect(img.src).toContain('test.jpg');
    expect(img.alt).toBe('Test image');
  });

  it('should not display image when imageUrl is not provided', () => {
    component.title = 'Test';
    fixture.detectChanges();
    const imgContainer = compiled.querySelector('.sc-card__image-container');
    expect(imgContainer).toBeFalsy();
  });

  it('should display category when provided', () => {
    component.category = 'Technology';
    component.title = 'Test';
    fixture.detectChanges();
    const category = compiled.querySelector('.sc-card__category');
    expect(category?.textContent).toBe('Technology');
  });

  it('should display description when provided', () => {
    component.description = 'Test description';
    component.title = 'Test';
    fixture.detectChanges();
    const description = compiled.querySelector('.sc-card__description');
    expect(description?.textContent).toBe('Test description');
  });

  it('should display link when provided', () => {
    component.link = '/test';
    component.linkText = 'Learn More';
    component.title = 'Test';
    fixture.detectChanges();
    const link = compiled.querySelector('.sc-card__link') as HTMLAnchorElement;
    expect(link).toBeTruthy();
    expect(link.href).toContain('/test');
    expect(link.textContent).toContain('Learn More');
  });

  it('should apply default variant class', () => {
    component.title = 'Test';
    fixture.detectChanges();
    const card = compiled.querySelector('.sc-card');
    expect(card?.classList.contains('sc-card--default')).toBe(true);
  });

  it('should apply horizontal variant class', () => {
    component.variant = 'horizontal';
    component.title = 'Test';
    fixture.detectChanges();
    const card = compiled.querySelector('.sc-card');
    expect(card?.classList.contains('sc-card--horizontal')).toBe(true);
  });

  it('should apply minimal variant class', () => {
    component.variant = 'minimal';
    component.title = 'Test';
    fixture.detectChanges();
    const card = compiled.querySelector('.sc-card');
    expect(card?.classList.contains('sc-card--minimal')).toBe(true);
  });
});
