import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';
import { FuseModule } from '@fuse';
import { FuseConfigModule } from '@fuse/services/config';
import { FuseMockApiModule } from '@fuse/lib/mock-api';
import { CoreModule } from 'app/core/core.module';
import { appConfig } from 'app/core/config/app.config';
import { mockApiServices } from 'app/mock-api';
import { LayoutModule } from 'app/layout/layout.module';
import { AppComponent } from 'app/app.component';
import { appRoutes } from 'app/app.routing';
import { AuthGuard } from './core/auth/guards/auth.guard';
import { PickerModule } from '@ctrl/ngx-emoji-mart';
import { ToastrModule } from 'ngx-toastr';
import { PostDetailsComponent } from './modules/posts/post-details/post-details.component';
import { PostsListComponent } from './modules/posts/list/list.component';

import { FuseCardModule } from '@fuse/components/card';
import { MatLuxonDateModule } from '@angular/material-luxon-adapter';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FuseDrawerModule } from '@fuse/components/drawer';
import { SharedModule } from './shared/shared.module';
import { MatRadioModule } from '@angular/material/radio';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatRippleModule } from '@angular/material/core';
import { CommentsComponent } from './modules/posts/comments/comments.component';
import { PostCommentsComponent } from './modules/posts/post-comments/post-comments.component';
import { LikesComponent } from './modules/posts/likes/likes.component';
import { ReplyComponent } from './modules/posts/reply/reply.component';
import { BrandsService } from './core/services-api/brands.service';


const routerConfig: ExtraOptions = {
    preloadingStrategy       : PreloadAllModules,
    scrollPositionRestoration: 'enabled'
};

@NgModule({
    declarations: [
        AppComponent,
       
    //    PostsListComponent,
        PostCommentsComponent,
        // LikesComponent,
        // ReplyComponent,
    ],
    bootstrap: [
        AppComponent
    ],
    providers: [
        AuthGuard
      ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        RouterModule.forRoot(appRoutes, routerConfig),
        // Fuse, FuseConfig & FuseMockAPI
        FuseModule,
        FuseConfigModule.forRoot(appConfig),
        FuseMockApiModule.forRoot(mockApiServices),
        // Core module of your application
        CoreModule,

        // Layout module of your application
        LayoutModule,
        PickerModule,
        ToastrModule.forRoot(), // Add this line
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
       /// PostsDetailsComponent,
       
    ]
})
export class AppModule
{
}
