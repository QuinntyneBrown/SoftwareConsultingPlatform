import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, timer } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export const retryInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const { maxRetries, retryDelayMs, retryStatusCodes } = environment.retryConfig;

  return next(req).pipe(
    retry({
      count: maxRetries,
      delay: (error: HttpErrorResponse, retryCount: number) => {
        // Only retry on specific status codes (server errors)
        if (!retryStatusCodes.includes(error.status)) {
          return throwError(() => error);
        }

        // Exponential backoff
        const delay = retryDelayMs * Math.pow(2, retryCount - 1);
        console.log(`Retrying request (attempt ${retryCount}/${maxRetries}) after ${delay}ms`);
        return timer(delay);
      }
    }),
    catchError((error: HttpErrorResponse) => {
      console.error('Request failed after all retries:', error);
      return throwError(() => error);
    })
  );
};
