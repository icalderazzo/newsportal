import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'news-paginator',
  templateUrl: './newspaginator.component.html'
})
export class NewspaginatorComponent {
  /**
   * Total items number.
   */
  @Input() length: number | undefined;

  /**
   * Sends a tuple with the page number to change and the current page size.
   */
  @Output() changePageEvent = new EventEmitter<[number, number]>();

  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10]
  
  pageEvent : PageEvent | undefined;

  handlePageEvent(e: PageEvent) {
    this.pageEvent = e;
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;

    this.changePageEvent.emit([this.pageIndex+1, this.pageSize]);
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }
}