import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'sc-card',
  imports: [CommonModule],
  templateUrl: './card.html',
  styleUrl: './card.scss',
})
export class Card {
  @Input() imageUrl?: string;
  @Input() imageAlt = '';
  @Input() category?: string;
  @Input() title = '';
  @Input() description?: string;
  @Input() link?: string;
  @Input() linkText = 'Read More';
  @Input() variant: 'default' | 'horizontal' | 'minimal' = 'default';

  get cardClasses(): string {
    const classes = ['sc-card'];
    classes.push(`sc-card--${this.variant}`);
    return classes.join(' ');
  }
}
