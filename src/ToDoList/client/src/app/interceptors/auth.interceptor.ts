import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import {
  HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { isPlatformBrowser } from '@angular/common';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authReq = req;

    if (isPlatformBrowser(this.platformId)) {
      const token = this.authService.getAccessToken();
      if (token) {
        authReq = req.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        });
      }
    }

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && isPlatformBrowser(this.platformId)) {
          return this.authService.refreshToken()!.pipe(
            switchMap(() => {
              const newToken = this.authService.getAccessToken();
              const retryReq = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` }
              });
              return next.handle(retryReq);
            })
          );
        }
        return throwError(() => error);
      })
    );
  }
}
