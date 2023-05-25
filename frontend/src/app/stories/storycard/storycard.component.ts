import { Component, Input } from '@angular/core';

@Component({
  selector: 'story-card',
  templateUrl: './storycard.component.html'
})
export class StorycardComponent {
  @Input() title: string | undefined;
  @Input() text: string | undefined;
  @Input() url: string | undefined;
}
