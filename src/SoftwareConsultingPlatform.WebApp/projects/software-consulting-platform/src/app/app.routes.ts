import { Routes } from '@angular/router';
import { Landing } from './pages/landing/landing';
import { CaseStudies } from './pages/case-studies/case-studies';
import { CaseStudyDetail } from './pages/case-study-detail/case-study-detail';
import { Services } from './pages/services/services';
import { ServiceDetail } from './pages/service-detail/service-detail';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { ForgotPassword } from './pages/forgot-password/forgot-password';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'case-studies', component: CaseStudies },
  { path: 'case-studies/:id', component: CaseStudyDetail },
  { path: 'services', component: Services },
  { path: 'services/:id', component: ServiceDetail },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'forgot-password', component: ForgotPassword },
  { path: '**', redirectTo: '' },
];
