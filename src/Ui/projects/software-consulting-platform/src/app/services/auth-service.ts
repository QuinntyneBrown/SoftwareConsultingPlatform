import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  companyName?: string;
  acceptTerms: boolean;
}

export interface PasswordResetRequest {
  email: string;
}

export interface PasswordResetConfirm {
  token: string;
  newPassword: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: User;
}

export interface User {
  userId: string;
  email: string;
  fullName: string;
  companyName?: string;
  avatar?: string;
  roles: string[];
}

export interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  loading: boolean;
  error: string | null;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  private authStateSubject = new BehaviorSubject<AuthState>({
    isAuthenticated: false,
    user: null,
    loading: false,
    error: null,
  });

  authState$ = this.authStateSubject.asObservable();
  isAuthenticated$ = this.authState$.pipe(map((state) => state.isAuthenticated));
  currentUser$ = this.authState$.pipe(map((state) => state.user));

  login(request: LoginRequest): Observable<AuthResponse> {
    this.setLoading(true);
    return this.http.post<AuthResponse>(`${this.baseUrl}/api/auth/login`, request).pipe(
      tap((response) => {
        this.handleAuthSuccess(response);
      }),
      catchError((error) => {
        this.setError(error.error?.message || 'Login failed. Please check your credentials.');
        throw error;
      })
    );
  }

  register(request: RegisterRequest): Observable<{ message: string }> {
    this.setLoading(true);
    return this.http.post<{ message: string }>(`${this.baseUrl}/api/auth/register`, request).pipe(
      tap(() => {
        this.setLoading(false);
      }),
      catchError((error) => {
        this.setError(error.error?.message || 'Registration failed. Please try again.');
        throw error;
      })
    );
  }

  requestPasswordReset(request: PasswordResetRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/api/auth/forgot-password`, request).pipe(
      catchError(() => of({ message: 'If an account exists with this email, you will receive a reset link.' }))
    );
  }

  resetPassword(request: PasswordResetConfirm): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/api/auth/reset-password`, request).pipe(
      catchError((error) => {
        throw error.error?.message || 'Password reset failed. The link may have expired.';
      })
    );
  }

  logout(): void {
    this.http.post(`${this.baseUrl}/api/auth/logout`, {}).subscribe({
      complete: () => {
        this.clearAuth();
      },
      error: () => {
        this.clearAuth();
      },
    });
  }

  refreshToken(): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/api/auth/refresh`, {}).pipe(
      tap((response) => {
        this.handleAuthSuccess(response);
      }),
      catchError(() => {
        this.clearAuth();
        throw new Error('Session expired');
      })
    );
  }

  loginWithOAuth(provider: 'google' | 'microsoft' | 'github'): void {
    window.location.href = `${this.baseUrl}/api/auth/oauth/${provider}`;
  }

  private handleAuthSuccess(response: AuthResponse): void {
    this.authStateSubject.next({
      isAuthenticated: true,
      user: response.user,
      loading: false,
      error: null,
    });
  }

  private clearAuth(): void {
    this.authStateSubject.next({
      isAuthenticated: false,
      user: null,
      loading: false,
      error: null,
    });
  }

  private setLoading(loading: boolean): void {
    this.authStateSubject.next({
      ...this.authStateSubject.getValue(),
      loading,
      error: null,
    });
  }

  private setError(error: string): void {
    this.authStateSubject.next({
      ...this.authStateSubject.getValue(),
      loading: false,
      error,
    });
  }
}
