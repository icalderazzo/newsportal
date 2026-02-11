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
    searchNews: (searchString: string, pageNumber:number , pageSize: number) => `/stories?searchString=${searchString}&pageNumber=${pageNumber}&pageSize=${pageSize}`,
    getBookmarks: (pageNumber:number , pageSize: number) => `/stories/bookmarks?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    bookmarkItem: (newsId: number) => `/stories/${newsId}/bookmark`
  }

  constructor(private http: HttpClient) { }

  public getLatestNews(pageNumber = this.defaultPageNumber, pageSize = this.defaultPageSize): Observable<NewsPortalPagedResponse<News[]>>{
    return this.http.get<NewsPortalPagedResponse<News[]>>(`${this.baseUrl}${this.API_ROUTES.getLatestNews(pageNumber, pageSize)}`);
  }

  public searchNews(searchString: string, pageNumber = this.defaultPageNumber, pageSize = this.defaultPageSize): Observable<NewsPortalPagedResponse<News[]>>{

    return this.http.get<NewsPortalPagedResponse<News[]>>(`${this.baseUrl}${this.API_ROUTES.searchNews(searchString, pageNumber, pageSize)}`);
  }

  public getBookmarks(pageNumber = this.defaultPageNumber, pageSize = this.defaultPageSize): Observable<NewsPortalPagedResponse<News[]>>{
    return this.http.get<NewsPortalPagedResponse<News[]>>(`${this.baseUrl}${this.API_ROUTES.getBookmarks(pageNumber, pageSize)}`);
  }

  public bookmarkItem(newsId: number): Observable<any>{
    return this.http.post(`${this.baseUrl}${this.API_ROUTES.bookmarkItem(newsId)}`, {});
  }

  public deleteBookmark(newsId: number): Observable<any>{
    return this.http.delete(`${this.baseUrl}${this.API_ROUTES.bookmarkItem(newsId)}`);
  }
}
