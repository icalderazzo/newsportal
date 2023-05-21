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
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { NewstableComponent } from './newstable/newstable.component';
import { NewssearchbarComponent } from './newssearchbar/newssearchbar.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    NewsComponent,
    NewscardComponent,
    NewspaginatorComponent,
    NewstableComponent,
    NewssearchbarComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatButtonModule,
    MatDividerModule,
    MatInputModule,
    MatIconModule,
    FormsModule
  ],
  exports:[
    NewsComponent
  ]
})
export class NewsModule { }
