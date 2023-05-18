import { Component, Input } from '@angular/core';

@Component({
  selector: 'news-card',
  templateUrl: './newscard.component.html'
})
export class NewscardComponent {
  @Input() title: string | undefined;
  @Input() text: string | undefined;
  @Input() url: string | undefined;
}
