import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-forgot-password',
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss',
})
export class ForgotPassword {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  isLoading$ = new BehaviorSubject<boolean>(false);
  success$ = new BehaviorSubject<boolean>(false);
  error$ = new BehaviorSubject<string | null>(null);

  forgotPasswordForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });

  onSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      this.isLoading$.next(true);
      this.error$.next(null);

      this.authService.requestPasswordReset(this.forgotPasswordForm.value).subscribe({
        next: () => {
          this.isLoading$.next(false);
          this.success$.next(true);
        },
        error: () => {
          this.isLoading$.next(false);
          this.success$.next(true);
        },
      });
    } else {
      this.forgotPasswordForm.markAllAsTouched();
    }
  }
}
