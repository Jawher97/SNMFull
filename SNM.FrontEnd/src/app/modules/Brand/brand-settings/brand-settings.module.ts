import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { Route, RouterModule } from '@angular/router';
import { ChannelResolver } from 'app/core/resolvers/brand.resolvers';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';
import { FuseHighlightModule } from '@fuse/components/highlight';
import { FuseAlertModule } from '@fuse/components/alert';
import { FuseNavigationModule } from '@fuse/components/navigation';
import { FuseScrollResetModule } from '@fuse/directives/scroll-reset';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { ChannelsComponent } from 'app/modules/Brand/brand-settings/channels/channels.component';
// import { FacebookPagesComponent } from 'app/modules/Brand/brand-settings/channels/facebook/facebook-pages/facebook-pages.component';
import { FacebookLoginComponent } from 'app/modules/Brand/brand-settings/channels/facebook/facebook-login/facebook-login.component';
import { MembersComponent } from 'app/modules/Brand/brand-settings/members/members.component';
import { PublishingComponent } from 'app/modules/Brand/brand-settings/publishing/publishing.component';
import { NotificationsComponent } from 'app/modules/Brand/brand-settings/notifications/notifications.component';
import { RolesPermissionsComponent } from 'app/modules/Brand/brand-settings/role-permission/roles-permissions.component';
import { BrandInformationComponent } from 'app/modules/Brand/brand-settings/brand-information/brand-information.component';
// import { LinkedinCompanyPagesComponent } from './channels/linkedin/linkedin-company-pages/linkedin-company-pages.component';
import { LinkedinCallbackComponent } from './channels/linkedin/linkedin-callback/linkedin-callback.component';
import { TwittercallbackComponent } from './channels/twitter/twitter-callback/twittercallback.component';
import { TwitterloginComponent } from './channels/twitter/twitter-login/twitterlogin.component';
import { LoginComponent } from './channels/linkedin/linkedin-login/login.component';
import { InstagramComponent } from './channels/instagram/instagram.component';
import { InstagramloginComponent } from './channels/instagram/instagramlogin/instagramlogin.component';
import { FacebookGroupComponent } from './channels/facebook/facebook-group/facebook-group.component';
import { LinkedinProfileConnectionComponent } from './channels/linkedin/linkedin-profile-connection/linkedin-profile-connection.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LinkedinProfileChannelComponent } from './channels/linkedin/linkedin-profile-channel/linkedin-profile-channel.component';
import { SocialChannelComponent } from './channels/social-channel/social-channel.component';
import { SocialChannelCallbackComponent } from './channels/social-channel-callback/social-channel-callback.component';
import { ManagedSocialChannelComponent } from './channels/managed-social-channel/managed-social-channel.component';
import { PickerModule } from '@ctrl/ngx-emoji-mart';
import { EmojiModule } from '@ctrl/ngx-emoji-mart/ngx-emoji';
import { PostsListComponent } from 'app/modules/posts/list/list.component';
import { PostDetailsComponent } from 'app/modules/posts/post-details/post-details.component';
import { LikesComponent } from 'app/modules/posts/likes/likes.component';
import { ReplyComponent } from 'app/modules/posts/reply/reply.component';
import { SummaryComponent } from 'app/modules/posts/summary/summary.component';



const brandRoutes: Route[] = [
  {
    path: '', pathMatch: 'full', redirectTo: 'channels'
  },
  {
    path: 'brand-information',
    component: BrandInformationComponent
  },
  {
    path: 'channels',
    component: ChannelsComponent,
    resolve: {
      //  channel: ChannelResolver,
    },
  },
  {
    path: 'members',
    component: MembersComponent
  },
  {
    path: 'publishing',
    component: PublishingComponent
  },
  {
    path: 'roles-permissions',
    component: RolesPermissionsComponent
  },
  {
    path: 'notifications',
    component: NotificationsComponent
  },
  {
    path: 'Posts',
    component: PostsListComponent
  },
  {
    path: 'Summary',
    component: SummaryComponent
  }
];
@NgModule({
  declarations: [
    BrandInformationComponent,
    ChannelsComponent,
    PostsListComponent,
    PostDetailsComponent,
    SummaryComponent,
    LikesComponent,
    ReplyComponent,
    // FacebookPagesComponent,
    FacebookLoginComponent,
    LoginComponent,
    // LinkedinCompanyPagesComponent,
    LinkedinCallbackComponent,
    TwitterloginComponent,
    TwittercallbackComponent,
    InstagramComponent,
    InstagramloginComponent,
    MembersComponent,
    PublishingComponent,
    RolesPermissionsComponent,
    NotificationsComponent,
    FacebookGroupComponent,
    LinkedinProfileConnectionComponent,
    LinkedinProfileChannelComponent,
    SocialChannelComponent,
    SocialChannelCallbackComponent,
    ManagedSocialChannelComponent,
    
  ],
  imports: [
    RouterModule.forChild(brandRoutes),
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatButtonModule, 
    MatCheckboxModule,
    MatDividerModule,
    FuseHighlightModule,
    FuseAlertModule,
    FuseNavigationModule,
    FuseScrollResetModule,
    FuseCardModule,
    SharedModule, PickerModule,EmojiModule, NgChartsModule
  ]
})
export class BrandSettingsModule { }
