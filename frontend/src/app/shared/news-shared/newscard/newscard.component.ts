import { Component, EventEmitter, Input, Output } from '@angular/core';
import { News } from 'src/app/core/models/newsportal/news';

@Component({
  selector: 'news-card',
  templateUrl: './newscard.component.html',
  styleUrl: './newscard.component.css',
  standalone: false
})
export class NewscardComponent {
  @Input() news!: News;
  @Input() mode: 'stories' | 'bookmarks' = 'stories';
  @Output() bookmarkToggled: EventEmitter<News> = new EventEmitter<News>();

  toggleBookmark(): void {
    this.bookmarkToggled.emit(this.news);
    this.news.isBookmarked = !this.news.isBookmarked;
  }
}
