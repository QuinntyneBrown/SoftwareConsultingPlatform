import { Component, Input, OnInit, OnDestroy, inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Testimonial } from '../testimonial/testimonial';

export interface TestimonialData {
  quote: string;
  authorName: string;
  authorPosition: string;
  authorCompany: string;
  avatarUrl?: string;
  rating?: number;
}

@Component({
  selector: 'sc-testimonial-carousel',
  imports: [CommonModule, Testimonial],
  templateUrl: './testimonial-carousel.html',
  styleUrl: './testimonial-carousel.scss',
})
export class TestimonialCarousel implements OnInit, OnDestroy {
  @Input() testimonials: TestimonialData[] = [];
  @Input() autoPlayInterval = 6000;
  @Input() sectionTitle = '';

  currentIndex = 0;
  private intervalId: ReturnType<typeof setInterval> | null = null;
  private platformId = inject(PLATFORM_ID);

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId) && this.testimonials.length > 1) {
      this.startAutoPlay();
    }
  }

  ngOnDestroy(): void {
    this.stopAutoPlay();
  }

  next(): void {
    this.currentIndex = (this.currentIndex + 1) % this.testimonials.length;
    this.restartAutoPlay();
  }

  previous(): void {
    this.currentIndex = (this.currentIndex - 1 + this.testimonials.length) % this.testimonials.length;
    this.restartAutoPlay();
  }

  goToSlide(index: number): void {
    this.currentIndex = index;
    this.restartAutoPlay();
  }

  private startAutoPlay(): void {
    this.intervalId = setInterval(() => {
      this.next();
    }, this.autoPlayInterval);
  }

  private stopAutoPlay(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  private restartAutoPlay(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.stopAutoPlay();
      this.startAutoPlay();
    }
  }
}
