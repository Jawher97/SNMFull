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
// import { PostsDetailsComponent } from './details/details.component';
import { FuseCardModule } from '@fuse/components/card';
import { MatLuxonDateModule } from '@angular/material-luxon-adapter';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommentsComponent } from './comments/comments.component';
import { CommentComponent } from './comment/comment/comment.component';
import { CommentFormComponent } from './commentForm/comment-form/comment-form.component';
import { PickerModule } from '@ctrl/ngx-emoji-mart';
import { CreatePostsComponent } from './create-posts/create-posts.component';
import { PostsHomeComponent } from './posts-home/posts-home.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { FuseDrawerModule } from "../../../@fuse/components/drawer/drawer.module";
import { InstagramPostDetailsComponent } from './instagram-post-details/instagram-post-details.component';
import { FacebookPostDetailsComponent } from './facebook-post-details/facebook-post-details.component';
import { LinkedinPostCommentsComponent } from './linkedin-post/linkedin-post-comments/linkedin-post-comments.component';
// import { LinkedinPostDetailsComponent } from './linkedin-post/linkedin-post-details/linkedin-post-details.component';
import { LinkedinPostCommentComponent } from './linkedin-post/linkedin-post-comment/linkedin-post-comment.component';
import { LinkedinPostCommentFormComponent } from './linkedin-post/linkedin-post-comment-form/linkedin-post-comment-form.component';
import { ToastrModule } from 'ngx-toastr';
import { ReplyComponent } from './reply/reply.component';
import { SummaryComponent } from './summary/summary.component';

@NgModule({
    declarations: [
        PostsComponent,
        
      
        CommentsComponent,
        CommentComponent,
        CommentFormComponent,
        CreatePostsComponent,
        PostsHomeComponent,
      
        InstagramPostDetailsComponent,
        FacebookPostDetailsComponent,
        // LinkedinPostDetailsComponent,
        LinkedinPostCommentsComponent,
        LinkedinPostCommentComponent,
        LinkedinPostCommentFormComponent,
    
       
        
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
        MatDatepickerModule,
        MatLuxonDateModule,
        MatSelectModule,
        NgxMatDatetimePickerModule,
        NgxMatTimepickerModule,
        NgxMatNativeDateModule,
        MatTooltipModule,
        MatButtonToggleModule,
        MatDividerModule,
        MatProgressBarModule,
        FuseDrawerModule,
        PickerModule,
       /// PostsDetailsComponent,
        ToastrModule.forRoot(),
    ]
})
export class PostsModule { }
