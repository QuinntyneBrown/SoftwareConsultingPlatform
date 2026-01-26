import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatChipsModule } from '@angular/material/chips';
import { combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import {
  Navigation,
  Footer,
  Button,
  CaseStudyGrid,
  CtaSection,
  Testimonial,
} from 'software-consulting-platform-components';
import { ServicesService } from '../../services/services-service';
import { LandingService } from '../../services/landing-service';

@Component({
  selector: 'app-service-detail',
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatExpansionModule,
    MatChipsModule,
    Navigation,
    Footer,
    Button,
    CaseStudyGrid,
    CtaSection,
    Testimonial,
  ],
  templateUrl: './service-detail.html',
  styleUrl: './service-detail.scss',
})
export class ServiceDetail {
  private route = inject(ActivatedRoute);
  private servicesService = inject(ServicesService);
  private landingService = inject(LandingService);

  viewModel$ = combineLatest([
    this.route.params.pipe(
      switchMap((params) => this.servicesService.getServiceById(params['id']))
    ),
    this.landingService.getLandingPageData(),
  ]).pipe(
    map(([service, landingData]) => ({
      service,
      navLinks: landingData.navLinks,
      footerColumns: landingData.footerColumns,
      footerTagline: landingData.footerTagline,
      copyright: landingData.copyright,
    }))
  );
}
