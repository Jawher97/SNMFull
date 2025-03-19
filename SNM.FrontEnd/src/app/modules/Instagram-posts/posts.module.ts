import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PostsRoutingModule } from './posts-routing.module';
import { PostsComponent } from './posts.component';
import { PostsListComponent } from './list/list.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatRippleModule } from '@angular/material/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { SharedModule } from 'app/shared/shared.module';
import { PostsDetailsComponent } from './details/details.component';
import { FuseCardModule } from '@fuse/components/card';
import { MatLuxonDateModule } from '@angular/material-luxon-adapter';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { MatTooltipModule } from '@angular/material/tooltip';
import { InstacommentsComponent } from './instacomments/instacomments.component';
import { InstacommentComponent } from './instacomment/instacomment.component';
import { CommentformComponent } from './commentform/commentform.component';

@NgModule({
  declarations: [
    PostsComponent,
    PostsListComponent,
    PostsDetailsComponent,
    InstacommentsComponent,
    InstacommentComponent,
    CommentformComponent
  ],
  imports: [
    CommonModule,
    PostsRoutingModule,
    MatButtonModule,
        MatCheckboxModule,
        MatDialogModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatRippleModule,
        MatSidenavModule,
        FuseMasonryModule,
        FuseCardModule,
        MatRadioModule,
        SharedModule,

        // MatButtonModule,
        // MatButtonToggleModule,
        // MatChipsModule,
        MatDatepickerModule,
        // MatDividerModule,
       
        // MatIconModule,
        // MatInputModule,
         MatLuxonDateModule,
        // MatMenuModule,
         MatSelectModule,
        // FuseHighlightModule,
        NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    NgxMatNativeDateModule,
    MatTooltipModule,
  


  ]
})
export class PostsModule { }
