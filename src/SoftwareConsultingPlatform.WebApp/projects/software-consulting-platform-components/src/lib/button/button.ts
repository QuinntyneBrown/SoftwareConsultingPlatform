import { Component, Input, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type ButtonVariant = 'primary' | 'secondary' | 'tertiary' | 'danger' | 'success';
export type ButtonSize = 'small' | 'medium' | 'large';

@Component({
  selector: 'sc-button',
  imports: [CommonModule],
  templateUrl: './button.html',
  styleUrl: './button.scss',
})
export class Button {
  @Input() variant: ButtonVariant = 'primary';
  @Input() size: ButtonSize = 'medium';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() fullWidth = false;
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() ariaLabel?: string;

  get buttonClasses(): string {
    const classes = ['sc-button'];
    classes.push(`sc-button--${this.variant}`);
    classes.push(`sc-button--${this.size}`);
    if (this.fullWidth) classes.push('sc-button--full-width');
    if (this.loading) classes.push('sc-button--loading');
    return classes.join(' ');
  }
}
