import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { Story } from '../models/story';

@Injectable({
  providedIn: 'root'
})
export class StoryService {
  backendUrl: string = `${environment.backendUrl}/api`;
  validationUrl: string = environment.validationUrl;

  constructor(private http: HttpClient) { }

  public validate(story: Story): Observable<HttpResponse<void>> {
    const url: string = `${this.validationUrl}/validation`;

    return this.http.post<HttpResponse<void>>(url, story);
  }

  public visualize(story: Story): Observable<HttpResponse<void>> {
    const url: string = `${this.backendUrl}/story`;

    return this.http.post<HttpResponse<void>>(url, story);
  }
}
