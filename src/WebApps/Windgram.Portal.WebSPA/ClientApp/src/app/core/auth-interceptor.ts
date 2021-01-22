import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { from, Observable } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { AuthService } from './services/auth.service';
import { User } from 'oidc-client';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private authService: AuthService
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const tokenSource = from(this.authService.getUser());
        return tokenSource.pipe(
            switchMap((user => {
                let request = req;
                if (user) {
                    request = request.clone({
                        setHeaders: {
                            Authorization: `${user.token_type} ${user.access_token}`
                        }
                    });
                }
                return next.handle(request);
            }))
        ).pipe(
            tap(
                (event: HttpEvent<any>) => {
                },
                (errorResponse: HttpErrorResponse) => {
                    console.log(errorResponse);
                }
            )
        );
    }
}
