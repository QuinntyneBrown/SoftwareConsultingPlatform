import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { switchMap, map } from 'rxjs/operators';
import { combineLatest } from 'rxjs';
import {
  Navigation,
  Footer,
  Button,
  CaseStudyGrid,
  CtaSection,
} from 'software-consulting-platform-components';
import { CaseStudiesService } from '../../services/case-studies-service';
import { LandingService } from '../../services/landing-service';

@Component({
  selector: 'app-case-study-detail',
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatChipsModule,
    MatButtonModule,
    MatDialogModule,
    Navigation,
    Footer,
    Button,
    CaseStudyGrid,
    CtaSection,
  ],
  templateUrl: './case-study-detail.html',
  styleUrl: './case-study-detail.scss',
})
export class CaseStudyDetail {
  private route = inject(ActivatedRoute);
  private caseStudiesService = inject(CaseStudiesService);
  private landingService = inject(LandingService);
  private dialog = inject(MatDialog);

  selectedImageIndex = 0;
  lightboxOpen = false;

  viewModel$ = combineLatest([
    this.route.params.pipe(
      switchMap((params) => this.caseStudiesService.getCaseStudyById(params['id']))
    ),
    this.landingService.getLandingPageData(),
  ]).pipe(
    map(([caseStudy, landingData]) => ({
      caseStudy,
      navLinks: landingData.navLinks,
      footerColumns: landingData.footerColumns,
      footerTagline: landingData.footerTagline,
      copyright: landingData.copyright,
    }))
  );

  openLightbox(index: number): void {
    this.selectedImageIndex = index;
    this.lightboxOpen = true;
  }

  closeLightbox(): void {
    this.lightboxOpen = false;
  }

  previousImage(totalImages: number): void {
    this.selectedImageIndex = (this.selectedImageIndex - 1 + totalImages) % totalImages;
  }

  nextImage(totalImages: number): void {
    this.selectedImageIndex = (this.selectedImageIndex + 1) % totalImages;
  }

  onKeyDown(event: KeyboardEvent, totalImages: number): void {
    if (!this.lightboxOpen) return;

    if (event.key === 'Escape') {
      this.closeLightbox();
    } else if (event.key === 'ArrowLeft') {
      this.previousImage(totalImages);
    } else if (event.key === 'ArrowRight') {
      this.nextImage(totalImages);
    }
  }

  shareOnLinkedIn(title: string, url: string): void {
    const shareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(url)}`;
    window.open(shareUrl, '_blank', 'width=600,height=400');
  }

  shareOnTwitter(title: string, url: string): void {
    const shareUrl = `https://twitter.com/intent/tweet?text=${encodeURIComponent(title)}&url=${encodeURIComponent(url)}`;
    window.open(shareUrl, '_blank', 'width=600,height=400');
  }

  shareOnFacebook(url: string): void {
    const shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}`;
    window.open(shareUrl, '_blank', 'width=600,height=400');
  }
}
