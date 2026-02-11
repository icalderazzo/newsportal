import { Component, OnInit, ViewChild } from '@angular/core';
import { News } from '../core/models/newsportal/news';
import { NewsPortalPagedResponse } from '../core/models/newsportal/newsPortalPagedResponse';
import { NewsportalService } from '../core/services/newsportal.service';
import { NewspaginatorComponent } from '../shared/news-shared/newspaginator/newspaginator.component';

@Component({
    selector: 'app-stories',
    templateUrl: './stories.component.html',
    styleUrls: ['./stories.component.css'],
    standalone: false
})
export class StoriesComponent implements OnInit {
  
  constructor(
    private newsPortalService: NewsportalService
  ) {}

  @ViewChild(NewspaginatorComponent) paginator: NewspaginatorComponent | undefined;

  newsResponse: NewsPortalPagedResponse<News[]> | undefined;  
  currentNews: News[] | undefined;
  totalNewsCount: number | undefined;
  loading = true;
  searchString: string | undefined;
  showSearchBar = false;

  ngOnInit(): void {
    this.loadNews();
  }

  loadNews(pageNumber = 1, pageSize = 5)
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
    this.paginator!.pageIndex = 0;
    this.loadNews();
  }

  onBookmarkToggled(news: News) {
    try {
      if(news.isbookmarked) {
        this.newsPortalService.deleteBookmark(news.id).subscribe();
      } else {
        this.newsPortalService.bookmarkItem(news.id).subscribe();
      }
    } catch (error) {
      console.log(error);
    }
  }
}
