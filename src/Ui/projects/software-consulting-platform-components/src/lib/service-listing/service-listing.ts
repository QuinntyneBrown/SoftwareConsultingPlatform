import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Service {
  icon?: string;
  title: string;
  description: string;
  features?: string[];
  link?: string;
}

@Component({
  selector: 'sc-service-listing',
  imports: [CommonModule],
  templateUrl: './service-listing.html',
  styleUrl: './service-listing.scss',
})
export class ServiceListing {
  @Input() services: Service[] = [];
  @Input() columns: number = 3;
}
