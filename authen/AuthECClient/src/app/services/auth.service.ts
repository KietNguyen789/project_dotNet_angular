import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.models';
import { AuthUtils } from './auth.utils';
@Injectable({ providedIn: 'root' })
export class AuthService {
  private base = `sys_user.ctr`;
  private _authenticated: boolean = false
  constructor(private http: HttpClient) { }

  register(payload: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/sign_up`, payload);
  }

  set accessToken(token: string) {
    localStorage.setItem("accessToken", token);
  }

  get accessToken() {
    return localStorage.getItem("accessToken") ?? '';
  }

  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/Authenticate`, payload);
  }

  check_login(): Observable<boolean> {
    // Check if the user is logged in
    if (this._authenticated) {
      return of(true);
    }

    // Check the access token availability
    if (!this.accessToken) {
      return of(false);
    }

    // Check the access token expire date
    if (AuthUtils.isTokenExpired(this.accessToken)) {


      return of(false);
    }
    console.log("token expire", AuthUtils.isTokenExpired(this.accessToken));

    // If the access token exists and it didn't expire, sign in using it
    return of(true);
  }
}
