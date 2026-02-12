import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoriesComponent } from './stories/stories.component';

const routes: Routes = [
  { path: '', redirectTo: 'stories', pathMatch: 'full' },
  { path: 'stories', component: StoriesComponent },
  { path: 'bookmarks', loadChildren: () => import('./bookmarks/bookmarks.module').then(m => m.BookmarksModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
