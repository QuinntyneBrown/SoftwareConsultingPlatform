import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface CaseStudy {
  imageUrl: string;
  imageAlt?: string;
  tags: string[];
  title: string;
  description: string;
  link: string;
  metrics?: { label: string; value: string }[];
}

@Component({
  selector: 'sc-case-study-grid',
  imports: [CommonModule],
  templateUrl: './case-study-grid.html',
  styleUrl: './case-study-grid.scss',
})
export class CaseStudyGrid {
  @Input() caseStudies: CaseStudy[] = [];
  @Input() columns: number = 3;
}
