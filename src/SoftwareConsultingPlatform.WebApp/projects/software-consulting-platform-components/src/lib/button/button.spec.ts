import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Button } from './button';

describe('Button', () => {
  let component: Button;
  let fixture: ComponentFixture<Button>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Button],
    }).compileComponents();

    fixture = TestBed.createComponent(Button);
    component = fixture.componentInstance;
    compiled = fixture.nativeElement;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Variants', () => {
    it('should apply primary variant class by default', () => {
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--primary')).toBe(true);
    });

    it('should apply secondary variant class', () => {
      component.variant = 'secondary';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--secondary')).toBe(true);
    });

    it('should apply tertiary variant class', () => {
      component.variant = 'tertiary';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--tertiary')).toBe(true);
    });

    it('should apply danger variant class', () => {
      component.variant = 'danger';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--danger')).toBe(true);
    });

    it('should apply success variant class', () => {
      component.variant = 'success';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--success')).toBe(true);
    });
  });

  describe('Sizes', () => {
    it('should apply medium size class by default', () => {
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--medium')).toBe(true);
    });

    it('should apply small size class', () => {
      component.size = 'small';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--small')).toBe(true);
    });

    it('should apply large size class', () => {
      component.size = 'large';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--large')).toBe(true);
    });
  });

  describe('States', () => {
    it('should be enabled by default', () => {
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.disabled).toBe(false);
    });

    it('should be disabled when disabled input is true', () => {
      component.disabled = true;
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.disabled).toBe(true);
    });

    it('should be disabled when loading', () => {
      component.loading = true;
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.disabled).toBe(true);
    });

    it('should show spinner when loading', () => {
      component.loading = true;
      fixture.detectChanges();
      const spinner = compiled.querySelector('.sc-button__spinner');
      expect(spinner).toBeTruthy();
    });

    it('should hide content when loading', () => {
      component.loading = true;
      fixture.detectChanges();
      const content = compiled.querySelector('.sc-button__content--hidden');
      expect(content).toBeTruthy();
    });

    it('should set aria-busy when loading', () => {
      component.loading = true;
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.getAttribute('aria-busy')).toBe('true');
    });
  });

  describe('Full Width', () => {
    it('should not have full-width class by default', () => {
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--full-width')).toBe(false);
    });

    it('should apply full-width class when fullWidth is true', () => {
      component.fullWidth = true;
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.classList.contains('sc-button--full-width')).toBe(true);
    });
  });

  describe('Button Type', () => {
    it('should have button type by default', () => {
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.type).toBe('button');
    });

    it('should apply submit type', () => {
      component.type = 'submit';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.type).toBe('submit');
    });

    it('should apply reset type', () => {
      component.type = 'reset';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.type).toBe('reset');
    });
  });

  describe('Accessibility', () => {
    it('should apply aria-label when provided', () => {
      component.ariaLabel = 'Custom label';
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.getAttribute('aria-label')).toBe('Custom label');
    });

    it('should not have aria-label when not provided', () => {
      fixture.detectChanges();
      const button = compiled.querySelector('button');
      expect(button?.getAttribute('aria-label')).toBeNull();
    });
  });

  describe('Content Projection', () => {
    it('should project content', () => {
      const testContent = 'Click Me';
      fixture = TestBed.createComponent(Button);
      const buttonElement = fixture.nativeElement.querySelector('button');
      buttonElement.textContent = testContent;
      fixture.detectChanges();
      expect(buttonElement.textContent).toContain(testContent);
    });
  });
});
