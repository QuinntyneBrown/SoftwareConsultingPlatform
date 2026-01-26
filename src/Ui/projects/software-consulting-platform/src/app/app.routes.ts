import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./layouts/main-layout/main-layout').then(m => m.MainLayout),
    children: [
      {
        path: '',
        loadComponent: () => import('./pages/landing/landing').then(m => m.Landing)
      },
      {
        path: 'case-studies',
        loadComponent: () => import('./pages/case-studies/case-studies').then(m => m.CaseStudies)
      },
      {
        path: 'case-studies/:id',
        loadComponent: () => import('./pages/case-study-detail/case-study-detail').then(m => m.CaseStudyDetail)
      },
      {
        path: 'services',
        loadComponent: () => import('./pages/services/services').then(m => m.Services)
      },
      {
        path: 'services/:id',
        loadComponent: () => import('./pages/service-detail/service-detail').then(m => m.ServiceDetail)
      },
      { path: '**', redirectTo: '' },
    ]
  }
];
