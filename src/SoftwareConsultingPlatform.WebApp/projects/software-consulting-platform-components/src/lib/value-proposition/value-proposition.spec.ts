import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ValueProposition } from './value-proposition';

describe('ValueProposition', () => {
  let component: ValueProposition;
  let fixture: ComponentFixture<ValueProposition>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ValueProposition],
    }).compileComponents();

    fixture = TestBed.createComponent(ValueProposition);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render section title when provided', () => {
    component.sectionTitle = 'Why Choose Us';
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.sc-value-proposition__title')?.textContent).toContain('Why Choose Us');
  });

  it('should render propositions', () => {
    component.propositions = [
      { icon: '🚀', headline: 'Fast Delivery', description: 'Quick turnaround' },
      { icon: '💡', headline: 'Innovation', description: 'Cutting edge solutions' },
    ];
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    const items = compiled.querySelectorAll('.sc-value-proposition__item');
    expect(items.length).toBe(2);
  });

  it('should apply variant class', () => {
    component.variant = 'dark';
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.sc-value-proposition--dark')).toBeTruthy();
  });
});
