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

  defaultPageNumber = 1;
  defaultPageSize = 5;

  private readonly API_ROUTES = {
    getLatestNews: (pageNumber:number , pageSize: number) => `/stories?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    searchNews: (searchString: string, pageNumber:number , pageSize: number) => `/stories?searchString=${searchString}&pageNumber=${pageNumber}&pageSize=${pageSize}`
  }

  constructor(private http: HttpClient) { }

  public getLatestNews(pageNumber = this.defaultPageNumber, pageSize = this.defaultPageSize): Observable<NewsPortalPagedResponse<News[]>>{
    return this.http.get<NewsPortalPagedResponse<News[]>>(`${this.baseUrl}${this.API_ROUTES.getLatestNews(pageNumber, pageSize)}`);
  }

  public searchNews(searchString: string, pageNumber = this.defaultPageNumber, pageSize = this.defaultPageSize): Observable<NewsPortalPagedResponse<News[]>>{

    return this.http.get<NewsPortalPagedResponse<News[]>>(`${this.baseUrl}${this.API_ROUTES.searchNews(searchString, pageNumber, pageSize)}`);
  }
}
