import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http'

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { StoriesComponent } from './stories/stories.component';
import { NewsSharedModule } from './shared/news-shared/news-shared.module';
import { ConfirmationDialogComponent } from './shared/confirmation-dialog/confirmation-dialog.component';

@NgModule({ declarations: [
        AppComponent,
        StoriesComponent,
        ConfirmationDialogComponent
    ],
    bootstrap: [AppComponent], 
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatToolbarModule,
        MatButtonModule,
        MatIconModule,
        MatDividerModule,
        MatDialogModule,
        MatSnackBarModule,
        NewsSharedModule], 
    providers: [provideHttpClient(withInterceptorsFromDi())] })

    export class AppModule { }
