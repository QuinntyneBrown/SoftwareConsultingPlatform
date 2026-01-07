import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface NavLink {
  label: string;
  routerLink: string;
}

@Component({
  selector: 'sc-navigation',
  imports: [CommonModule, RouterModule],
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
