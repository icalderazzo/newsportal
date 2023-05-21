import { Component, Input, OnInit } from '@angular/core';
import { News } from '../core/models/newsportal/news';
import { NewsPortalPagedResponse } from '../core/models/newsportal/newsPortalPagedResponse';
import { NewsportalService } from '../core/services/newsportal.service';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
  
  constructor(private newsPortalService: NewsportalService) {}
  
  @Input() showSearchBar : boolean | undefined;

  newsResponse: NewsPortalPagedResponse<News[]> | undefined;  
  currentNews: News[] | undefined;
  totalNewsCount: number | undefined;
  loading = true;
  searchString: string | undefined;

  ngOnInit(): void {
    this.loadNews();
  }

  async loadNews(pageNumber = 1, pageSize = 5)
  {
    this.loading = true;
    try {
      (this.searchString?
        this.newsPortalService.searchNews(this.searchString, pageNumber, pageSize)
        : this.newsPortalService.getLatestNews(pageNumber, pageSize)
      ).subscribe((response) => {
        this.newsResponse = response;
        this.totalNewsCount = response.totalRecords;
        this.currentNews = response.data;
      });
    } catch (error) {
      console.log(error);  
    } finally {
      this.loading = false;
    }
  }

  changePage($event: any) {
    this.loadNews($event[0], $event[1])
  }
  
  search($event: any) {
    this.searchString = $event;
    this.loadNews();
  }
}
