import { Component, Input } from '@angular/core';
import { News } from 'src/app/core/models/newsportal/news';

@Component({
  selector: 'news-table',
  templateUrl: './newstable.component.html',
  styleUrls: ['./newstable.component.css']
})
export class NewstableComponent {
  @Input() loading: boolean | undefined;
  @Input() news : News[] | undefined;
}
