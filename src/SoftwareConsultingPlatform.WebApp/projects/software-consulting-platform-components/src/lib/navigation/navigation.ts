import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface NavLink {
  label: string;
  href: string;
}

@Component({
  selector: 'sc-navigation',
  imports: [CommonModule],
  templateUrl: './navigation.html',
  styleUrl: './navigation.scss',
})
export class Navigation {
  @Input() logoSrc?: string;
  @Input() logoAlt = 'Logo';
  @Input() links: NavLink[] = [];
  @Input() sticky = true;
  mobileMenuOpen = false;

  toggleMobileMenu(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }
}
