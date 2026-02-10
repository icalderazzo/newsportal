import { Component, EventEmitter, Output } from '@angular/core';

@Component({
    selector: 'stories-searchbar',
    templateUrl: './storiessearchbar.component.html',
    standalone: false
})
export class StoriessearchbarComponent {
  @Output() searchNewsEvent = new EventEmitter<string>();

  searchString: string | undefined;

  handleSearchEvent()
  {
    this.searchNewsEvent.emit(this.searchString);
  }
}
