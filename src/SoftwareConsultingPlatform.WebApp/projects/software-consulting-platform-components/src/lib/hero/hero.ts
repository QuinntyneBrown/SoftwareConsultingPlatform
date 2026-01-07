import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'sc-hero',
  imports: [CommonModule],
  templateUrl: './hero.html',
  styleUrl: './hero.scss',
})
export class Hero {
  @Input() headline = '';
  @Input() subheadline = '';
  @Input() backgroundImage?: string;
  @Input() alignment: 'left' | 'center' = 'left';
}
