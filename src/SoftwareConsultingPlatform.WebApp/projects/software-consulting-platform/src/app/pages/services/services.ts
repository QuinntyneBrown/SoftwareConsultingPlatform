import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  Navigation,
  Footer,
  Hero,
  Button,
  CtaSection,
} from 'software-consulting-platform-components';
import { ServicesService } from '../../services/services-service';
import { LandingService } from '../../services/landing-service';

@Component({
  selector: 'app-services',
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatCardModule,
    Navigation,
    Footer,
    Hero,
    Button,
    CtaSection,
  ],
  templateUrl: './services.html',
  styleUrl: './services.scss',
})
export class Services {
  private servicesService = inject(ServicesService);
  private landingService = inject(LandingService);

  viewModel$ = combineLatest([
    this.servicesService.getServicesOverview(),
    this.landingService.getLandingPageData(),
  ]).pipe(
    map(([servicesData, landingData]) => ({
      heroHeadline: servicesData.heroHeadline,
      heroSubheadline: servicesData.heroSubheadline,
      services: servicesData.services,
      navLinks: landingData.navLinks,
      footerColumns: landingData.footerColumns,
      footerTagline: landingData.footerTagline,
      copyright: landingData.copyright,
    }))
  );
}
