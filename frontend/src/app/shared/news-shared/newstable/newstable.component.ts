import { Component, EventEmitter, Input, Output } from '@angular/core';
import { News } from 'src/app/core/models/newsportal/news';

@Component({
  selector: 'news-table',
  templateUrl: './newstable.component.html',
  styleUrl: './newstable.component.css',
  standalone: false,
})
export class NewstableComponent {
  @Input() loading: boolean | undefined;
  @Input() news: News[] | undefined;
  @Input() mode: 'stories' | 'bookmarks' = 'stories';
  @Output() bookmarkToggled: EventEmitter<News> = new EventEmitter<News>();

  onBookmarkToggled(news: News) {
    this.bookmarkToggled.emit(news);
  }
}
