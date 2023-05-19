import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { NewsPortalPagedResponse } from '../models/newsportal/newsPortalPagedResponse';
import { News } from '../models/newsportal/news';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsportalService {
  
  baseUrl = environment.apis.newsportal.baseUrl;

  private readonly API_ROUTES = {
    getLatestNews: (pageNumber:number , pageSize: number) => `/stories?pageNumber=${pageNumber}&pageSize=${pageSize}`
  }

  constructor(private http: HttpClient) { }

  public getLatestNews(pageNumber = 1, pageSize = 5): Observable<NewsPortalPagedResponse<News[]>>{
    return this.http.get<NewsPortalPagedResponse<News[]>>(`${this.baseUrl}${this.API_ROUTES.getLatestNews(pageNumber, pageSize)}`);
  }
}
