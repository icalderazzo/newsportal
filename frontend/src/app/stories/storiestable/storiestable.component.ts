import { Component, Input } from '@angular/core';
import { News } from 'src/app/core/models/newsportal/news';

@Component({
  selector: 'stories-table',
  templateUrl: './storiestable.component.html'
})
export class StoriestableComponent {
  @Input() loading: boolean | undefined;
  @Input() news : News[] | undefined;
}
