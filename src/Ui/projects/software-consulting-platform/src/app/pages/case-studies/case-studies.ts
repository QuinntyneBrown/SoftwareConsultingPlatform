import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatBadgeModule } from '@angular/material/badge';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  Navigation,
  Footer,
  Hero,
  Button,
  CaseStudyGrid,
} from 'software-consulting-platform-components';
import { CaseStudiesService, SortOption } from '../../services/case-studies-service';
import { LandingService } from '../../services/landing-service';

@Component({
  selector: 'app-case-studies',
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatIconModule,
    MatChipsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatExpansionModule,
    MatBadgeModule,
    MatPaginatorModule,
    Navigation,
    Footer,
    Hero,
    Button,
    CaseStudyGrid,
  ],
  templateUrl: './case-studies.html',
  styleUrl: './case-studies.scss',
})
export class CaseStudies {
  private caseStudiesService = inject(CaseStudiesService);
  private landingService = inject(LandingService);

  sortOptions: { value: SortOption; label: string }[] = [
    { value: 'date-newest', label: 'Date (Newest)' },
    { value: 'date-oldest', label: 'Date (Oldest)' },
    { value: 'client-az', label: 'Client Name (A-Z)' },
  ];

  viewModel$ = combineLatest([
    this.caseStudiesService.getCaseStudies(),
    this.caseStudiesService.searchTerm$,
    this.caseStudiesService.selectedIndustries$,
    this.caseStudiesService.selectedTechnologies$,
    this.caseStudiesService.selectedServiceTypes$,
    this.caseStudiesService.sortOption$,
    this.caseStudiesService.page$,
    this.landingService.getLandingPageData(),
  ]).pipe(
    map(([response, searchTerm, industries, technologies, serviceTypes, sortOption, page, landingData]) => ({
      caseStudies: response.caseStudies,
      totalCount: response.totalCount,
      filters: response.filters,
      searchTerm,
      selectedIndustries: industries,
      selectedTechnologies: technologies,
      selectedServiceTypes: serviceTypes,
      sortOption,
      page,
      hasActiveFilters: searchTerm.length > 0 || industries.length > 0 || technologies.length > 0 || serviceTypes.length > 0,
      navLinks: landingData.navLinks,
      footerColumns: landingData.footerColumns,
      footerTagline: landingData.footerTagline,
      copyright: landingData.copyright,
    }))
  );

  onSearchChange(term: string): void {
    this.caseStudiesService.setSearchTerm(term);
  }

  onIndustryToggle(industry: string): void {
    this.caseStudiesService.toggleIndustry(industry);
  }

  onTechnologyToggle(tech: string): void {
    this.caseStudiesService.toggleTechnology(tech);
  }

  onServiceTypeToggle(serviceType: string): void {
    this.caseStudiesService.toggleServiceType(serviceType);
  }

  onSortChange(option: SortOption): void {
    this.caseStudiesService.setSortOption(option);
  }

  onClearFilters(): void {
    this.caseStudiesService.clearAllFilters();
  }

  onPageChange(event: PageEvent): void {
    this.caseStudiesService.setPage(event.pageIndex + 1);
  }
}
