import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of, BehaviorSubject, combineLatest } from 'rxjs';
import { catchError, map, debounceTime, switchMap, startWith } from 'rxjs/operators';
import { CaseStudy } from 'software-consulting-platform-components';
import { environment } from '../../environments/environment';

export interface CaseStudyDetail extends CaseStudy {
  caseStudyId: string;
  clientName: string;
  clientLogo?: string;
  projectDuration: string;
  overview: string;
  challenge: string;
  solution: string;
  results: string;
  technologies: CaseStudyTechnology[];
  screenshots: Screenshot[];
  testimonial?: TestimonialQuote;
  relatedCaseStudies: CaseStudy[];
}

export interface CaseStudyTechnology {
  name: string;
  icon?: string;
  category: string;
}

export interface Screenshot {
  url: string;
  alt: string;
  caption?: string;
}

export interface TestimonialQuote {
  quote: string;
  authorName: string;
  authorPosition: string;
  authorCompany: string;
}

export interface CaseStudyFilter {
  industries: string[];
  technologies: string[];
  serviceTypes: string[];
}

export interface CaseStudyListResponse {
  caseStudies: CaseStudy[];
  totalCount: number;
  filters: CaseStudyFilter;
}

export type SortOption = 'date-newest' | 'date-oldest' | 'client-az';

@Injectable({
  providedIn: 'root',
})
export class CaseStudiesService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  private searchTermSubject = new BehaviorSubject<string>('');
  private selectedIndustriesSubject = new BehaviorSubject<string[]>([]);
  private selectedTechnologiesSubject = new BehaviorSubject<string[]>([]);
  private selectedServiceTypesSubject = new BehaviorSubject<string[]>([]);
  private sortOptionSubject = new BehaviorSubject<SortOption>('date-newest');
  private pageSubject = new BehaviorSubject<number>(1);

  searchTerm$ = this.searchTermSubject.asObservable();
  selectedIndustries$ = this.selectedIndustriesSubject.asObservable();
  selectedTechnologies$ = this.selectedTechnologiesSubject.asObservable();
  selectedServiceTypes$ = this.selectedServiceTypesSubject.asObservable();
  sortOption$ = this.sortOptionSubject.asObservable();
  page$ = this.pageSubject.asObservable();

  setSearchTerm(term: string): void {
    this.searchTermSubject.next(term);
    this.pageSubject.next(1);
  }

  toggleIndustry(industry: string): void {
    const current = this.selectedIndustriesSubject.getValue();
    const updated = current.includes(industry)
      ? current.filter((i) => i !== industry)
      : [...current, industry];
    this.selectedIndustriesSubject.next(updated);
    this.pageSubject.next(1);
  }

  toggleTechnology(tech: string): void {
    const current = this.selectedTechnologiesSubject.getValue();
    const updated = current.includes(tech)
      ? current.filter((t) => t !== tech)
      : [...current, tech];
    this.selectedTechnologiesSubject.next(updated);
    this.pageSubject.next(1);
  }

  toggleServiceType(serviceType: string): void {
    const current = this.selectedServiceTypesSubject.getValue();
    const updated = current.includes(serviceType)
      ? current.filter((s) => s !== serviceType)
      : [...current, serviceType];
    this.selectedServiceTypesSubject.next(updated);
    this.pageSubject.next(1);
  }

  setSortOption(option: SortOption): void {
    this.sortOptionSubject.next(option);
  }

  setPage(page: number): void {
    this.pageSubject.next(page);
  }

  clearAllFilters(): void {
    this.selectedIndustriesSubject.next([]);
    this.selectedTechnologiesSubject.next([]);
    this.selectedServiceTypesSubject.next([]);
    this.searchTermSubject.next('');
    this.pageSubject.next(1);
  }

  getCaseStudies(): Observable<CaseStudyListResponse> {
    return combineLatest([
      this.searchTerm$.pipe(debounceTime(300), startWith('')),
      this.selectedIndustries$,
      this.selectedTechnologies$,
      this.selectedServiceTypes$,
      this.sortOption$,
      this.page$,
    ]).pipe(
      switchMap(([search, industries, technologies, serviceTypes, sort, page]) => {
        let params = new HttpParams();
        if (search) params = params.set('search', search);
        if (industries.length) params = params.set('industries', industries.join(','));
        if (technologies.length) params = params.set('technologies', technologies.join(','));
        if (serviceTypes.length) params = params.set('serviceTypes', serviceTypes.join(','));
        params = params.set('sort', sort);
        params = params.set('page', page.toString());
        params = params.set('pageSize', '12');

        return this.http.get<CaseStudyListResponse>(`${this.baseUrl}/api/case-studies`, { params }).pipe(
          catchError(() => of(this.getDefaultCaseStudies(search, industries, technologies, serviceTypes, sort)))
        );
      })
    );
  }

  getCaseStudyById(id: string): Observable<CaseStudyDetail> {
    return this.http.get<CaseStudyDetail>(`${this.baseUrl}/api/case-studies/${id}`).pipe(
      catchError(() => of(this.getDefaultCaseStudyDetail(id)))
    );
  }

  private getDefaultCaseStudies(
    search: string,
    industries: string[],
    technologies: string[],
    serviceTypes: string[],
    sort: SortOption
  ): CaseStudyListResponse {
    let caseStudies: CaseStudy[] = [
      {
        imageUrl: 'assets/case-study-healthcare.jpg',
        imageAlt: 'Healthcare platform dashboard',
        tags: ['Healthcare', 'Cloud', 'AI'],
        title: 'Healthcare Platform Modernization',
        description: 'Transformed a legacy system into a modern cloud-native platform serving 500k+ patients.',
        link: '/case-studies/healthcare-platform',
        metrics: [
          { label: 'Performance Improvement', value: '300%' },
          { label: 'Cost Reduction', value: '45%' },
        ],
      },
      {
        imageUrl: 'assets/case-study-fintech.jpg',
        imageAlt: 'Financial services dashboard',
        tags: ['Finance', 'Security', 'Microservices'],
        title: 'Financial Services API Platform',
        description: 'Built a secure, scalable API platform processing millions of transactions daily.',
        link: '/case-studies/fintech-api',
        metrics: [
          { label: 'API Transactions', value: '10M+/day' },
          { label: 'Uptime', value: '99.99%' },
        ],
      },
      {
        imageUrl: 'assets/case-study-ecommerce.jpg',
        imageAlt: 'E-commerce platform interface',
        tags: ['E-commerce', 'Performance', 'UX'],
        title: 'E-commerce Platform Optimization',
        description: 'Optimized performance and user experience for a major online retailer.',
        link: '/case-studies/ecommerce-optimization',
        metrics: [
          { label: 'Conversion Rate', value: '+85%' },
          { label: 'Load Time', value: '-60%' },
        ],
      },
      {
        imageUrl: 'assets/case-study-logistics.jpg',
        imageAlt: 'Logistics management system',
        tags: ['Logistics', 'IoT', 'Real-time'],
        title: 'Real-time Logistics Tracking',
        description: 'Developed an IoT-powered logistics platform with real-time fleet tracking.',
        link: '/case-studies/logistics-tracking',
        metrics: [
          { label: 'Delivery Accuracy', value: '99.5%' },
          { label: 'Fleet Efficiency', value: '+40%' },
        ],
      },
      {
        imageUrl: 'assets/case-study-education.jpg',
        imageAlt: 'Learning management system',
        tags: ['Education', 'SaaS', 'Scalability'],
        title: 'Learning Management System',
        description: 'Built a scalable LMS platform serving thousands of students worldwide.',
        link: '/case-studies/lms-platform',
        metrics: [
          { label: 'Active Users', value: '50K+' },
          { label: 'Course Completion', value: '+65%' },
        ],
      },
      {
        imageUrl: 'assets/case-study-manufacturing.jpg',
        imageAlt: 'Manufacturing automation dashboard',
        tags: ['Manufacturing', 'Automation', 'Analytics'],
        title: 'Smart Manufacturing Solution',
        description: 'Implemented Industry 4.0 solutions for predictive maintenance and optimization.',
        link: '/case-studies/smart-manufacturing',
        metrics: [
          { label: 'Downtime Reduction', value: '70%' },
          { label: 'Production Increase', value: '25%' },
        ],
      },
    ];

    if (search) {
      const searchLower = search.toLowerCase();
      caseStudies = caseStudies.filter(
        (cs) =>
          cs.title.toLowerCase().includes(searchLower) ||
          cs.description.toLowerCase().includes(searchLower) ||
          cs.tags.some((tag) => tag.toLowerCase().includes(searchLower))
      );
    }

    if (industries.length) {
      caseStudies = caseStudies.filter((cs) =>
        cs.tags.some((tag) => industries.includes(tag))
      );
    }

    if (technologies.length) {
      caseStudies = caseStudies.filter((cs) =>
        cs.tags.some((tag) => technologies.includes(tag))
      );
    }

    if (sort === 'client-az') {
      caseStudies.sort((a, b) => a.title.localeCompare(b.title));
    } else if (sort === 'date-oldest') {
      caseStudies.reverse();
    }

    return {
      caseStudies,
      totalCount: caseStudies.length,
      filters: {
        industries: ['Healthcare', 'Finance', 'E-commerce', 'Education', 'Manufacturing', 'Logistics'],
        technologies: ['Cloud', 'AI', 'Microservices', 'IoT', 'SaaS', 'Analytics'],
        serviceTypes: ['Custom Development', 'Cloud Solutions', 'Security', 'Digital Transformation'],
      },
    };
  }

  private getDefaultCaseStudyDetail(id: string): CaseStudyDetail {
    return {
      caseStudyId: id,
      imageUrl: 'assets/case-study-healthcare.jpg',
      imageAlt: 'Healthcare platform dashboard',
      tags: ['Healthcare', 'Cloud', 'AI'],
      title: 'Healthcare Platform Modernization',
      description: 'Transformed a legacy system into a modern cloud-native platform.',
      link: `/case-studies/${id}`,
      clientName: 'MedTech Solutions',
      clientLogo: 'assets/client-medtech.png',
      projectDuration: '8 months',
      overview:
        'MedTech Solutions needed to modernize their legacy patient management system to improve performance, scalability, and user experience while maintaining strict healthcare compliance standards.',
      challenge:
        'The existing system was built on outdated technology, suffering from performance issues during peak hours, and lacked the flexibility to integrate with modern healthcare APIs and third-party services.',
      solution:
        'We redesigned the entire platform using a microservices architecture on Azure, implementing event-driven patterns for real-time updates, and built a modern React frontend with comprehensive accessibility features.',
      results:
        'The new platform achieved 300% performance improvement, 45% cost reduction in infrastructure, and received positive feedback from over 500,000 patients and healthcare providers.',
      metrics: [
        { label: 'Performance Improvement', value: '300%' },
        { label: 'Cost Reduction', value: '45%' },
        { label: 'Patient Satisfaction', value: '95%' },
        { label: 'System Uptime', value: '99.99%' },
      ],
      technologies: [
        { name: 'Azure', icon: 'cloud', category: 'Cloud' },
        { name: 'Kubernetes', icon: 'container', category: 'DevOps' },
        { name: '.NET Core', icon: 'code', category: 'Backend' },
        { name: 'React', icon: 'web', category: 'Frontend' },
        { name: 'PostgreSQL', icon: 'database', category: 'Database' },
        { name: 'Redis', icon: 'cache', category: 'Caching' },
      ],
      screenshots: [
        { url: 'assets/screenshots/healthcare-1.jpg', alt: 'Dashboard overview', caption: 'Main dashboard' },
        { url: 'assets/screenshots/healthcare-2.jpg', alt: 'Patient records', caption: 'Patient management' },
        { url: 'assets/screenshots/healthcare-3.jpg', alt: 'Analytics view', caption: 'Analytics dashboard' },
      ],
      testimonial: {
        quote:
          'The team delivered exceptional results, transforming our outdated systems into a modern, scalable platform that has significantly improved our operations.',
        authorName: 'Sarah Chen',
        authorPosition: 'CTO',
        authorCompany: 'MedTech Solutions',
      },
      relatedCaseStudies: [
        {
          imageUrl: 'assets/case-study-fintech.jpg',
          imageAlt: 'Financial services dashboard',
          tags: ['Finance', 'Security'],
          title: 'Financial Services API Platform',
          description: 'Built a secure, scalable API platform.',
          link: '/case-studies/fintech-api',
        },
        {
          imageUrl: 'assets/case-study-education.jpg',
          imageAlt: 'Learning management system',
          tags: ['Education', 'SaaS'],
          title: 'Learning Management System',
          description: 'Built a scalable LMS platform.',
          link: '/case-studies/lms-platform',
        },
      ],
    };
  }
}
