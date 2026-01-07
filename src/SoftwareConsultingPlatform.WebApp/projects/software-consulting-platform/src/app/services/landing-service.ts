import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {
  NavLink,
  FooterColumn,
  Service,
  CaseStudy,
  TestimonialData,
  ValuePropositionItem,
} from 'software-consulting-platform-components';
import { environment } from '../../environments/environment';

export interface LandingPageData {
  navLinks: NavLink[];
  heroHeadline: string;
  heroSubheadline: string;
  services: Service[];
  caseStudies: CaseStudy[];
  valuePropositions: ValuePropositionItem[];
  testimonials: TestimonialData[];
  footerColumns: FooterColumn[];
  footerTagline: string;
  copyright: string;
}

@Injectable({
  providedIn: 'root',
})
export class LandingService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  getLandingPageData(): Observable<LandingPageData> {
    return this.http.get<LandingPageData>(`${this.baseUrl}/api/landing`).pipe(
      catchError(() => of(this.getDefaultData()))
    );
  }

  private getDefaultData(): LandingPageData {
    return {
      navLinks: [
        { label: 'Home', href: '/' },
        { label: 'Services', href: '/services' },
        { label: 'Case Studies', href: '/case-studies' },
        { label: 'About', href: '/about' },
        { label: 'Contact', href: '/contact' },
      ],
      heroHeadline: 'Transform Your Business with Expert Software Consulting',
      heroSubheadline:
        'We deliver innovative solutions that drive growth, efficiency, and competitive advantage for forward-thinking organizations.',
      services: [
        {
          icon: '💻',
          title: 'Custom Development',
          description: 'Tailored software solutions built to your exact specifications.',
          features: ['Full-stack development', 'API integration', 'Cloud-native apps'],
          link: '/services/custom-development',
        },
        {
          icon: '☁️',
          title: 'Cloud Solutions',
          description: 'Scalable cloud infrastructure and migration services.',
          features: ['Azure & AWS', 'Kubernetes', 'DevOps'],
          link: '/services/cloud-solutions',
        },
        {
          icon: '🔒',
          title: 'Security Consulting',
          description: 'Protect your digital assets with enterprise-grade security.',
          features: ['Penetration testing', 'Compliance', 'Security audits'],
          link: '/services/security',
        },
        {
          icon: '📊',
          title: 'Data Analytics',
          description: 'Turn your data into actionable business insights.',
          features: ['BI dashboards', 'ML/AI solutions', 'Data pipelines'],
          link: '/services/analytics',
        },
        {
          icon: '🚀',
          title: 'Digital Transformation',
          description: 'Modernize your operations with cutting-edge technology.',
          features: ['Process automation', 'Legacy migration', 'Change management'],
          link: '/services/digital-transformation',
        },
        {
          icon: '🎯',
          title: 'Strategy & Advisory',
          description: 'Expert guidance for your technology roadmap.',
          features: ['Tech assessment', 'Architecture review', 'CTO advisory'],
          link: '/services/strategy',
        },
      ],
      caseStudies: [
        {
          imageUrl: 'assets/case-study-1.jpg',
          imageAlt: 'Healthcare platform dashboard',
          tags: ['Healthcare', 'Cloud', 'AI'],
          title: 'Healthcare Platform Modernization',
          description: 'Transformed a legacy system into a modern cloud-native platform.',
          link: '/case-studies/healthcare-platform',
          metrics: [
            { label: 'Performance Improvement', value: '300%' },
            { label: 'Cost Reduction', value: '45%' },
          ],
        },
        {
          imageUrl: 'assets/case-study-2.jpg',
          imageAlt: 'Financial services dashboard',
          tags: ['Finance', 'Security', 'Microservices'],
          title: 'Financial Services API Platform',
          description: 'Built a secure, scalable API platform for fintech innovation.',
          link: '/case-studies/fintech-api',
          metrics: [
            { label: 'API Transactions', value: '10M+/day' },
            { label: 'Uptime', value: '99.99%' },
          ],
        },
        {
          imageUrl: 'assets/case-study-3.jpg',
          imageAlt: 'E-commerce platform interface',
          tags: ['E-commerce', 'Performance', 'UX'],
          title: 'E-commerce Platform Optimization',
          description: 'Optimized performance and user experience for a major retailer.',
          link: '/case-studies/ecommerce-optimization',
          metrics: [
            { label: 'Conversion Rate', value: '+85%' },
            { label: 'Load Time', value: '-60%' },
          ],
        },
      ],
      valuePropositions: [
        {
          icon: '🏆',
          headline: 'Proven Expertise',
          description: 'Over 15 years of experience delivering complex software projects across industries.',
        },
        {
          icon: '🤝',
          headline: 'Partnership Approach',
          description: 'We work as an extension of your team, aligned with your business goals.',
        },
        {
          icon: '⚡',
          headline: 'Agile Delivery',
          description: 'Iterative development with rapid feedback cycles and transparent communication.',
        },
        {
          icon: '🎯',
          headline: 'Results-Focused',
          description: 'Measurable outcomes that drive real business value and ROI.',
        },
      ],
      testimonials: [
        {
          quote:
            'The team delivered exceptional results, transforming our outdated systems into a modern, scalable platform that has significantly improved our operations.',
          authorName: 'Sarah Chen',
          authorPosition: 'CTO',
          authorCompany: 'MedTech Solutions',
          rating: 5,
        },
        {
          quote:
            'Their expertise in cloud architecture helped us reduce costs by 40% while improving performance. Highly recommend for any enterprise project.',
          authorName: 'Michael Roberts',
          authorPosition: 'VP of Engineering',
          authorCompany: 'Global Finance Corp',
          rating: 5,
        },
        {
          quote:
            'Working with this team was a game-changer. They understood our needs perfectly and delivered ahead of schedule with outstanding quality.',
          authorName: 'Emily Thompson',
          authorPosition: 'CEO',
          authorCompany: 'RetailTech Inc',
          rating: 5,
        },
      ],
      footerColumns: [
        {
          title: 'Services',
          links: [
            { label: 'Custom Development', href: '/services/custom-development' },
            { label: 'Cloud Solutions', href: '/services/cloud-solutions' },
            { label: 'Security Consulting', href: '/services/security' },
            { label: 'Data Analytics', href: '/services/analytics' },
          ],
        },
        {
          title: 'Company',
          links: [
            { label: 'About Us', href: '/about' },
            { label: 'Case Studies', href: '/case-studies' },
            { label: 'Careers', href: '/careers' },
            { label: 'Blog', href: '/blog' },
          ],
        },
        {
          title: 'Contact',
          links: [
            { label: 'Get in Touch', href: '/contact' },
            { label: 'Support', href: '/support' },
            { label: 'Partners', href: '/partners' },
          ],
        },
      ],
      footerTagline: 'Transforming businesses through innovative software solutions.',
      copyright: `© ${new Date().getFullYear()} Software Consulting Platform. All rights reserved.`,
    };
  }
}
