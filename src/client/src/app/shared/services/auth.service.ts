import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { AuthResponse } from '../models/auth-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  authBaseUrl: string = `${environment.backendUrl}/api/auth`;

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  get isAuthenticated(): boolean {
    let token = localStorage.getItem('token');

    return token !== null ? true : false;
  }

  public login(email: string, password: string): Observable<any> {
    const url = `${this.authBaseUrl}/login`;

    return this.http.post<AuthResponse>(url, {
      email: email,
      password: password
    });
  }

  public register(email: string, name: string, password: string): Observable<AuthResponse> {
    const url = `${this.authBaseUrl}/register`;

    return this.http
      .post<AuthResponse>(url, {
        email: email,
        name: name,
        password: password
      });
  }

  public logout(): void {
      let token = localStorage.removeItem('token');

      if (token == null)
        this.router.navigate(['auth', 'signin']);
  }
}
