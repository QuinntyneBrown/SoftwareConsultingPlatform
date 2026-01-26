import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface ValuePropositionItem {
  icon: string;
  headline: string;
  description: string;
}

@Component({
  selector: 'sc-value-proposition',
  imports: [CommonModule],
  templateUrl: './value-proposition.html',
  styleUrl: './value-proposition.scss',
})
export class ValueProposition {
  @Input() propositions: ValuePropositionItem[] = [];
  @Input() sectionTitle = '';
  @Input() variant: 'light' | 'brand' | 'dark' = 'light';
}
