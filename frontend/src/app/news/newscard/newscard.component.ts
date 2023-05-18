import { Component, Input } from '@angular/core';

@Component({
  selector: 'news-card',
  templateUrl: './newscard.component.html',
  styleUrls: ['./newscard.component.css']
})
export class NewscardComponent {
  @Input() title: string | undefined;
  @Input() text: string | undefined;
  @Input() url: string | undefined;
}
