import { Component, OnInit } from '@angular/core';
import { News } from '../core/models/newsportal/news';
import { NewsPortalPagedResponse } from '../core/models/newsportal/newsPortalPagedResponse';
import { NewsportalService } from '../core/services/newsportal.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
  
  constructor(private newsPortalService: NewsportalService) {}
  
  newsResponse: NewsPortalPagedResponse<News[]> | undefined;  
  currentNews: News[] | undefined;
  totalNewsCount: number | undefined;

  ngOnInit(): void {
    this.loadNews();
  }

  loadNews(pageNumber = 1, pageSize = 10)
  {
    try {
      this.newsPortalService.getLatestNews(pageNumber, pageSize).subscribe((data) => {
        this.newsResponse = data;
        this.totalNewsCount = data.totalRecords;
        this.currentNews = data.data;
      }); 
    } catch (error) {
      console.log(error);  
    }
  }

  changePage($event: any) {
    this.loadNews($event[0], $event[1])
  }
}
