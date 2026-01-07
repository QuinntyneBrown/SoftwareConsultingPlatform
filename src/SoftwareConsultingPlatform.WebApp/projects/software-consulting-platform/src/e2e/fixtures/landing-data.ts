export const mockLandingPageData = {
  navLinks: [
    { label: 'Home', routerLink: '/' },
    { label: 'Services', routerLink: '/services' },
    { label: 'Case Studies', routerLink: '/case-studies' },
    { label: 'About', routerLink: '/about' },
    { label: 'Contact', routerLink: '/contact' },
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
  ],
  testimonials: [
    {
      quote:
        'The team delivered exceptional results, transforming our outdated systems into a modern, scalable platform.',
      authorName: 'Sarah Chen',
      authorPosition: 'CTO',
      authorCompany: 'MedTech Solutions',
      rating: 5,
    },
    {
      quote:
        'Their expertise in cloud architecture helped us reduce costs by 40% while improving performance.',
      authorName: 'Michael Roberts',
      authorPosition: 'VP of Engineering',
      authorCompany: 'Global Finance Corp',
      rating: 5,
    },
  ],
  footerColumns: [
    {
      title: 'Services',
      links: [
        { label: 'Custom Development', routerLink: '/services/custom-development' },
        { label: 'Cloud Solutions', routerLink: '/services/cloud-solutions' },
      ],
    },
    {
      title: 'Company',
      links: [
        { label: 'About Us', routerLink: '/about' },
        { label: 'Case Studies', routerLink: '/case-studies' },
      ],
    },
  ],
  footerTagline: 'Transforming businesses through innovative software solutions.',
  copyright: '© 2026 Software Consulting Platform. All rights reserved.',
};
