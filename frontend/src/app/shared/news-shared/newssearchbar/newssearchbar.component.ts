import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'news-searchbar',
  templateUrl: './newssearchbar.component.html',
  styleUrl: './newssearchbar.component.css',
  standalone: false
})
export class NewssearchbarComponent {
  @Output() searchNewsEvent = new EventEmitter<string>();

  searchString: string | undefined;

  handleSearchEvent()
  {
    this.searchNewsEvent.emit(this.searchString);
  }
}
