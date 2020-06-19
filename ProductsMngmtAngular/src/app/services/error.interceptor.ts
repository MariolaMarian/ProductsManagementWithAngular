import { Injectable, ErrorHandler } from '@angular/core';
import {
  HttpInterceptor,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: import('@angular/common/http').HttpRequest<any>,
    next: import('@angular/common/http').HttpHandler
  ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error) => {
        if (error.status === 401) {
          return throwError(error.statusText);
        }
        if (error.status === 403) {
          return throwError('You are not allowed to perform this action');
        }
        if (error instanceof HttpErrorResponse) {
          const apiError = error.headers.get('Application-Error');
          if (apiError) {
            return throwError(apiError);
          }
          const serverError = error.error;
          let modelStateErrors = '';

          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              if (serverError.errors[key]) {
                modelStateErrors += serverError.errors[key] + '\n';
              }
            }
          } else {
            if (serverError && typeof serverError === 'object') {
              for (const key in serverError) {
                if (serverError[key]) {
                  if (serverError[key].description === undefined) {
                    return throwError('Server is down');
                  } else {
                    modelStateErrors += serverError[key].description + '\n';
                  }
                }
              }
            }
          }

          return throwError(modelStateErrors || serverError || 'Unknown error');
        }
      })
    );
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true,
};
