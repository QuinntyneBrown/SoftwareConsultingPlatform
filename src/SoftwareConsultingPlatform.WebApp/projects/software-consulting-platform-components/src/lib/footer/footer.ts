import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface FooterLink {
  label: string;
  href: string;
}

export interface FooterColumn {
  title: string;
  links: FooterLink[];
}

@Component({
  selector: 'sc-footer',
  imports: [CommonModule],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class Footer {
  @Input() logoSrc?: string;
  @Input() tagline = '';
  @Input() columns: FooterColumn[] = [];
  @Input() copyright = '';
  @Input() theme: 'light' | 'dark' = 'light';
}
