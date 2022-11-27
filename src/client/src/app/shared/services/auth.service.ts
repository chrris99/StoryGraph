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

  get user(): string {
    let name = localStorage.getItem('user');

    return name !== null ? name : '';
  }

  public login(email: string, password: string): void {
    const url = `${this.authBaseUrl}/login`;

    this.http
      .post<AuthResponse>(url, {
        email: email,
        password: password
      })
      .subscribe((res: AuthResponse) => {
        this.saveUser(res);
      });
  }

  public register(email: string, name: string, password: string): void {
    const url = `${this.authBaseUrl}/register`;

    this.http
      .post<AuthResponse>(url, {
        email: email,
        name: name,
        password: password
      })
      .subscribe((res: AuthResponse) => {
        this.saveUser(res);
      });
  }

  public logout(): void {
      localStorage.removeItem('token');
      localStorage.removeItem('user');

      this.router.navigate(['/']);
  }

  private saveUser(res: AuthResponse): void {
    localStorage.setItem('token', res.token);
    localStorage.setItem('user', res.name);

    this.router.navigateByUrl('/');
  }
}
