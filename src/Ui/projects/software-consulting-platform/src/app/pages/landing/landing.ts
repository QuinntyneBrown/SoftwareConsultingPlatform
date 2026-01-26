import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { map } from 'rxjs';
import {
  Hero,
  Button,
  Navigation,
  Footer,
  ServiceListing,
  CaseStudyGrid,
  TestimonialCarousel,
  ValueProposition,
  CtaSection,
} from 'software-consulting-platform-components';
import { LandingService } from '../../services/landing-service';

@Component({
  selector: 'app-landing',
  imports: [
    CommonModule,
    Hero,
    Button,
    Navigation,
    Footer,
    ServiceListing,
    CaseStudyGrid,
    TestimonialCarousel,
    ValueProposition,
    CtaSection,
  ],
  templateUrl: './landing.html',
  styleUrl: './landing.scss',
})
export class Landing {
  private landingService = inject(LandingService);

  viewModel$ = this.landingService.getLandingPageData().pipe(
    map((data) => ({
      navLinks: data.navLinks,
      heroHeadline: data.heroHeadline,
      heroSubheadline: data.heroSubheadline,
      services: data.services,
      caseStudies: data.caseStudies,
      valuePropositions: data.valuePropositions,
      testimonials: data.testimonials,
      footerColumns: data.footerColumns,
      footerTagline: data.footerTagline,
      copyright: data.copyright,
    }))
  );
}
