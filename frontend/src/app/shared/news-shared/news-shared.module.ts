import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { NewstableComponent } from './newstable/newstable.component';
import { NewscardComponent } from './newscard/newscard.component';
import { NewspaginatorComponent } from './newspaginator/newspaginator.component';
import { NewssearchbarComponent } from './newssearchbar/newssearchbar.component';

@NgModule({
  declarations: [
    NewstableComponent,
    NewscardComponent,
    NewspaginatorComponent,
    NewssearchbarComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule
  ],
  exports: [
    NewstableComponent,
    NewscardComponent,
    NewspaginatorComponent,
    NewssearchbarComponent
  ]
})
export class NewsSharedModule { }
