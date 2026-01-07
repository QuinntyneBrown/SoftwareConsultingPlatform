import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface FooterLink {
  label: string;
  routerLink: string;
}

export interface FooterColumn {
  title: string;
  links: FooterLink[];
}

@Component({
  selector: 'sc-footer',
  imports: [CommonModule, RouterModule],
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
