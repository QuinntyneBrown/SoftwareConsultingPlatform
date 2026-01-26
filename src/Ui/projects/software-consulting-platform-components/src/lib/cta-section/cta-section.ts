import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'sc-cta-section',
  imports: [CommonModule],
  templateUrl: './cta-section.html',
  styleUrl: './cta-section.scss',
})
export class CtaSection {
  @Input() headline = '';
  @Input() subheadline = '';
  @Input() variant: 'standard' | 'brand' | 'dark' = 'standard';
}
