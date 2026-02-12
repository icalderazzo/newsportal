import { Component, OnInit, ViewChild } from '@angular/core';
import { News } from '../core/models/newsportal/news';
import { NewsPortalPagedResponse } from '../core/models/newsportal/newsPortalPagedResponse';
import { NewsportalService } from '../core/services/newsportal.service';
import { NewspaginatorComponent } from '../shared/news-shared/newspaginator/newspaginator.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrl: './bookmarks.component.css',
  standalone: false,
})
export class BookmarksComponent implements OnInit {
  constructor(
    private newsPortalService: NewsportalService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

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
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        data: {
          title: 'Remove Bookmark',
          message: 'Are you sure you want to remove this story from your bookmarks?',
          confirmText: 'Remove',
          cancelText: 'Cancel'
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.newsPortalService.deleteBookmark(news.id).subscribe(() => {
            this.snackBar.open('The story has been removed from your bookmarks', 'Close', {
              duration: 3000,
              horizontalPosition: 'end',
              verticalPosition: 'top',
              panelClass: ['success-snackbar']
            });
            this.loadNews();
          });
        }
      });
    } catch (error) {
      console.log(error);
    }
  }
}
