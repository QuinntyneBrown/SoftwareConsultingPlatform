import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Button, Navigation, NavLink, Footer } from 'software-consulting-platform-components';
import { LandingService } from '../../services/landing-service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-main-layout',
  imports: [CommonModule, RouterOutlet, Navigation, Button, Footer],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {
  private landingService = inject(LandingService);

  navLinks: NavLink[] = [
    { label: 'Home', routerLink: '/' },
    { label: 'Services', routerLink: '/services' },
    { label: 'Case Studies', routerLink: '/case-studies' },
    { label: 'About', routerLink: '/about' },
    { label: 'Contact', routerLink: '/contact' },
  ];

  viewModel$ = this.landingService.getLandingPageData().pipe(
    map((landingData) => ({
      footerColumns: landingData.footerColumns,
      footerTagline: landingData.footerTagline,
      copyright: landingData.copyright,
    }))
  );
}
