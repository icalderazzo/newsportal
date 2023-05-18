import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsComponent } from './news.component';
import { NewscardComponent } from './newscard/newscard.component';
import { NewspaginatorComponent } from './newspaginator/newspaginator.component';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule} from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';

@NgModule({
  declarations: [
    NewsComponent,
    NewscardComponent,
    NewspaginatorComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatButtonModule,
    MatDividerModule
  ],
  exports:[
    NewsComponent
  ]
})
export class NewsModule { }
