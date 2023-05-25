import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoriesComponent } from './stories.component';
import { StorycardComponent } from './storycard/storycard.component';
import { StoriespaginatorComponent } from './storiespaginator/storiespaginator.component'
import { StoriestableComponent } from './storiestable/storiestable.component';
import { StoriessearchbarComponent } from './storiessearchbar/storiessearchbar.component';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule} from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    StoriesComponent,
    StorycardComponent,
    StoriespaginatorComponent,
    StoriestableComponent,
    StoriessearchbarComponent
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
    StoriesComponent
  ]
})
export class StoriesModule { }
