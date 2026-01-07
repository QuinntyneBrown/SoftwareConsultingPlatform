import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Service, CaseStudy, TestimonialData } from 'software-consulting-platform-components';
import { environment } from '../../environments/environment';

export interface ServiceDetail extends Service {
  serviceId: string;
  tagline: string;
  heroImage: string;
  overview: string;
  activities: string[];
  processSteps: ProcessStep[];
  technologies: TechnologyGroup[];
  benefits: Benefit[];
  pricingModels: PricingModel[];
  faqs: FAQ[];
  relatedCaseStudies: CaseStudy[];
  testimonial?: TestimonialData;
  metrics: ServiceMetric[];
}

export interface ProcessStep {
  stepNumber: number;
  title: string;
  description: string;
  duration?: string;
}

export interface TechnologyGroup {
  category: string;
  items: ServiceTechnology[];
}

export interface ServiceTechnology {
  name: string;
  icon?: string;
}

export interface Benefit {
  icon: string;
  headline: string;
  description: string;
}

export interface PricingModel {
  name: string;
  description: string;
  idealFor: string;
}

export interface FAQ {
  question: string;
  answer: string;
}

export interface ServiceMetric {
  label: string;
  value: string;
}

export interface ServicesOverviewData {
  heroHeadline: string;
  heroSubheadline: string;
  services: Service[];
}

@Injectable({
  providedIn: 'root',
})
export class ServicesService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  getServicesOverview(): Observable<ServicesOverviewData> {
    return this.http.get<ServicesOverviewData>(`${this.baseUrl}/api/services`).pipe(
      catchError(() => of(this.getDefaultServicesOverview()))
    );
  }

  getServiceById(id: string): Observable<ServiceDetail> {
    return this.http.get<ServiceDetail>(`${this.baseUrl}/api/services/${id}`).pipe(
      catchError(() => of(this.getDefaultServiceDetail(id)))
    );
  }

  private getDefaultServicesOverview(): ServicesOverviewData {
    return {
      heroHeadline: 'Our Services',
      heroSubheadline: 'Comprehensive software consulting solutions tailored to your business needs.',
      services: [
        {
          icon: 'code',
          title: 'Custom Development',
          description: 'Tailored software solutions built to your exact specifications and business requirements.',
          features: ['Full-stack development', 'API integration', 'Cloud-native apps', 'Mobile development'],
          link: '/services/custom-development',
        },
        {
          icon: 'cloud',
          title: 'Cloud Solutions',
          description: 'Scalable cloud infrastructure, migration services, and cloud-native architecture.',
          features: ['Azure & AWS', 'Kubernetes', 'DevOps', 'Serverless'],
          link: '/services/cloud-solutions',
        },
        {
          icon: 'security',
          title: 'Security Consulting',
          description: 'Protect your digital assets with enterprise-grade security solutions.',
          features: ['Penetration testing', 'Compliance', 'Security audits', 'Identity management'],
          link: '/services/security',
        },
        {
          icon: 'analytics',
          title: 'Data Analytics',
          description: 'Transform your data into actionable business insights and intelligence.',
          features: ['BI dashboards', 'ML/AI solutions', 'Data pipelines', 'Visualization'],
          link: '/services/analytics',
        },
        {
          icon: 'rocket_launch',
          title: 'Digital Transformation',
          description: 'Modernize your operations with cutting-edge technology and processes.',
          features: ['Process automation', 'Legacy migration', 'Change management', 'Integration'],
          link: '/services/digital-transformation',
        },
        {
          icon: 'target',
          title: 'Strategy & Advisory',
          description: 'Expert guidance for your technology roadmap and strategic planning.',
          features: ['Tech assessment', 'Architecture review', 'CTO advisory', 'Due diligence'],
          link: '/services/strategy',
        },
      ],
    };
  }

  private getDefaultServiceDetail(id: string): ServiceDetail {
    const serviceDefaults: Record<string, Partial<ServiceDetail>> = {
      'custom-development': {
        title: 'Custom Development',
        tagline: 'Build software that perfectly fits your business',
        icon: 'code',
        overview:
          'Our custom development service delivers tailored software solutions designed specifically for your unique business requirements. We combine deep technical expertise with agile methodologies to create high-quality, maintainable applications that drive real business value.',
      },
      'cloud-solutions': {
        title: 'Cloud Solutions',
        tagline: 'Scale your infrastructure with confidence',
        icon: 'cloud',
        overview:
          'Our cloud solutions service helps organizations leverage the full power of cloud computing. From initial strategy and architecture design to migration and ongoing optimization, we ensure your cloud infrastructure is secure, scalable, and cost-effective.',
      },
      security: {
        title: 'Security Consulting',
        tagline: 'Protect what matters most',
        icon: 'security',
        overview:
          'Our security consulting service provides comprehensive protection for your digital assets. We identify vulnerabilities, implement robust security measures, and ensure compliance with industry standards and regulations.',
      },
      analytics: {
        title: 'Data Analytics',
        tagline: 'Turn data into decisions',
        icon: 'analytics',
        overview:
          'Our data analytics service transforms raw data into actionable insights. We help organizations build modern data platforms, implement advanced analytics, and leverage AI/ML to make data-driven decisions.',
      },
      'digital-transformation': {
        title: 'Digital Transformation',
        tagline: 'Reimagine your business for the digital age',
        icon: 'rocket_launch',
        overview:
          'Our digital transformation service helps organizations modernize their operations and embrace digital technologies. We guide you through process optimization, technology adoption, and cultural change.',
      },
      strategy: {
        title: 'Strategy & Advisory',
        tagline: 'Expert guidance for your technology journey',
        icon: 'target',
        overview:
          'Our strategy and advisory service provides expert guidance for your technology decisions. From roadmap development to architecture review, we help you make informed choices that align with your business goals.',
      },
    };

    const defaults = serviceDefaults[id] || serviceDefaults['custom-development'];

    return {
      serviceId: id,
      title: defaults.title || 'Custom Development',
      tagline: defaults.tagline || 'Build software that fits your business',
      icon: defaults.icon || 'code',
      description: 'Tailored software solutions built to your exact specifications.',
      features: ['Full-stack development', 'API integration', 'Cloud-native apps', 'Mobile development'],
      link: `/services/${id}`,
      heroImage: `assets/service-${id}.jpg`,
      overview: defaults.overview || 'Expert software development services.',
      activities: [
        'Requirements analysis and solution design',
        'Full-stack application development',
        'API design and integration',
        'Database design and optimization',
        'Quality assurance and testing',
        'Deployment and DevOps setup',
        'Documentation and knowledge transfer',
        'Ongoing maintenance and support',
      ],
      processSteps: [
        {
          stepNumber: 1,
          title: 'Discovery',
          description: 'We analyze your requirements and define project scope.',
          duration: '1-2 weeks',
        },
        {
          stepNumber: 2,
          title: 'Design',
          description: 'We create detailed technical specifications and architecture.',
          duration: '2-3 weeks',
        },
        {
          stepNumber: 3,
          title: 'Development',
          description: 'We build your solution using agile sprints with regular demos.',
          duration: '8-16 weeks',
        },
        {
          stepNumber: 4,
          title: 'Testing',
          description: 'We ensure quality through comprehensive testing.',
          duration: '2-4 weeks',
        },
        {
          stepNumber: 5,
          title: 'Deployment',
          description: 'We deploy to production and ensure smooth launch.',
          duration: '1-2 weeks',
        },
        {
          stepNumber: 6,
          title: 'Support',
          description: 'We provide ongoing maintenance and enhancements.',
          duration: 'Ongoing',
        },
      ],
      technologies: [
        {
          category: 'Frontend',
          items: [
            { name: 'Angular', icon: 'web' },
            { name: 'React', icon: 'web' },
            { name: 'TypeScript', icon: 'code' },
          ],
        },
        {
          category: 'Backend',
          items: [
            { name: '.NET Core', icon: 'code' },
            { name: 'Node.js', icon: 'code' },
            { name: 'Python', icon: 'code' },
          ],
        },
        {
          category: 'Cloud',
          items: [
            { name: 'Azure', icon: 'cloud' },
            { name: 'AWS', icon: 'cloud' },
            { name: 'Kubernetes', icon: 'cloud' },
          ],
        },
        {
          category: 'Database',
          items: [
            { name: 'PostgreSQL', icon: 'database' },
            { name: 'MongoDB', icon: 'database' },
            { name: 'Redis', icon: 'database' },
          ],
        },
      ],
      benefits: [
        {
          icon: 'verified',
          headline: 'Tailored Solutions',
          description: 'Software designed specifically for your unique business needs.',
        },
        {
          icon: 'speed',
          headline: 'Faster Time-to-Market',
          description: 'Agile development with rapid iteration and delivery.',
        },
        {
          icon: 'trending_up',
          headline: 'Scalable Architecture',
          description: 'Built to grow with your business demands.',
        },
        {
          icon: 'support_agent',
          headline: 'Dedicated Support',
          description: 'Ongoing maintenance and expert assistance.',
        },
      ],
      pricingModels: [
        {
          name: 'Time & Materials',
          description: 'Pay for actual hours worked with flexible scope. Best for evolving requirements.',
          idealFor: 'Projects with evolving requirements or ongoing development',
        },
        {
          name: 'Fixed Price',
          description: 'Set budget with defined deliverables. Best for well-defined projects.',
          idealFor: 'Projects with clear, stable requirements and defined scope',
        },
        {
          name: 'Retainer',
          description: 'Reserved capacity at discounted rates. Best for ongoing needs.',
          idealFor: 'Organizations needing consistent development support',
        },
      ],
      faqs: [
        {
          question: 'How long does a typical project take?',
          answer:
            'Project timelines vary based on complexity and scope. A typical MVP takes 3-4 months, while larger enterprise solutions may take 6-12 months. We provide detailed estimates during the discovery phase.',
        },
        {
          question: 'What technologies do you specialize in?',
          answer:
            'We specialize in modern web and cloud technologies including .NET, Angular, React, Azure, AWS, and Kubernetes. We select the best technology stack based on your specific requirements.',
        },
        {
          question: 'How do you ensure project quality?',
          answer:
            'We follow rigorous quality practices including code reviews, automated testing, CI/CD pipelines, and regular client demos. Our QA process includes unit, integration, and end-to-end testing.',
        },
        {
          question: 'Do you provide ongoing support after launch?',
          answer:
            'Yes, we offer various support and maintenance packages to ensure your solution continues to perform optimally. This includes bug fixes, security updates, and feature enhancements.',
        },
        {
          question: 'How do you handle project communication?',
          answer:
            'We use agile practices with regular sprint reviews, daily standups, and dedicated communication channels. You will have direct access to your development team and a dedicated project manager.',
        },
      ],
      relatedCaseStudies: [
        {
          imageUrl: 'assets/case-study-healthcare.jpg',
          imageAlt: 'Healthcare platform',
          tags: ['Healthcare', 'Cloud'],
          title: 'Healthcare Platform Modernization',
          description: 'Transformed legacy systems into modern cloud platform.',
          link: '/case-studies/healthcare-platform',
        },
        {
          imageUrl: 'assets/case-study-fintech.jpg',
          imageAlt: 'Fintech platform',
          tags: ['Finance', 'API'],
          title: 'Financial Services API Platform',
          description: 'Built secure, scalable API platform.',
          link: '/case-studies/fintech-api',
        },
      ],
      testimonial: {
        quote:
          'The development team delivered exceptional quality and truly understood our business needs. The custom solution has transformed how we operate.',
        authorName: 'Michael Roberts',
        authorPosition: 'VP of Engineering',
        authorCompany: 'TechCorp Inc',
        rating: 5,
      },
      metrics: [
        { label: 'Projects Delivered', value: '200+' },
        { label: 'Client Satisfaction', value: '98%' },
        { label: 'On-Time Delivery', value: '95%' },
      ],
    };
  }
}
