import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authService.accessToken;

        if (token) {
            debugger
            const clonedReq = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${token}`)
            });
            return next.handle(clonedReq).pipe(
                tap({
                    error: (err: any) => {
                        if (err.status === 401) {
                            // this.authService.deleteToken();
                            // this.router.navigate(['/login']);
                        } else if (err.status === 403) {
                            console.log('not authorized');
                        }
                    }
                })
            );
        }

        return next.handle(req);
    }
}
