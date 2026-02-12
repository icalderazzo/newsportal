import { Component, OnInit, ViewChild } from '@angular/core';
import { News } from '../core/models/newsportal/news';
import { NewsPortalPagedResponse } from '../core/models/newsportal/newsPortalPagedResponse';
import { NewsportalService } from '../core/services/newsportal.service';
import { NewspaginatorComponent } from '../shared/news-shared/newspaginator/newspaginator.component';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrl: './bookmarks.component.css',
  standalone: false,
})
export class BookmarksComponent implements OnInit {
  constructor(private newsPortalService: NewsportalService) {}

  @ViewChild(NewspaginatorComponent) paginator:
    | NewspaginatorComponent
    | undefined;

  newsResponse: NewsPortalPagedResponse<News[]> | undefined;
  currentNews: News[] | undefined;
  totalNewsCount: number | undefined;
  loading = true;
  searchString: string | undefined;

  ngOnInit(): void {
    this.loadNews();
  }

  loadNews(pageNumber = 1, pageSize = 5) {
    this.loading = true;
    try {
      this.newsPortalService
        .getBookmarks(pageNumber, pageSize)
        .subscribe((response) => {
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
    this.loadNews($event[0], $event[1]);
  }

  onDeleteBookmark(news: News) {
    try {
      this.newsPortalService.deleteBookmark(news.id).subscribe(() => {
        this.loadNews();
      });
    } catch (error) {
      console.log(error);
    }
  }
}
