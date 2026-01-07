import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'sc-testimonial',
  imports: [CommonModule],
  templateUrl: './testimonial.html',
  styleUrl: './testimonial.scss',
})
export class Testimonial {
  @Input() quote = '';
  @Input() authorName = '';
  @Input() authorPosition = '';
  @Input() authorCompany = '';
  @Input() avatarUrl?: string;
  @Input() rating?: number;

  get stars(): number[] {
    return this.rating ? Array(this.rating).fill(0) : [];
  }

  get emptyStars(): number[] {
    return this.rating ? Array(5 - this.rating).fill(0) : [];
  }
}
