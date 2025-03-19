import { Route } from '@angular/router';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { LayoutComponent } from 'app/layout/layout.component';
import { InitialDataResolver } from 'app/app.resolvers';

import { BrandsResolver } from './core/resolvers/brand.resolvers';
import { SocialChannelCallbackComponent } from './modules/Brand/brand-settings/channels/social-channel-callback/social-channel-callback.component';
import { PostsListComponent } from './modules/posts/list/list.component';


// @formatter:off
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/explicit-function-return-type */
export const appRoutes: Route[] = [

    // Redirect empty path to '/brand'
    {
        path: '',
         resolve: {
             brand: BrandsResolver,
        }, 
        pathMatch: 'full', redirectTo: 'brand'
    },

    // Redirect signed-in user to the '/example'
    //
    // After the user signs in, the sign-in page will redirect the user to the 'signed-in-redirect'
    // path. Below is another redirection for that path to redirect the user to the desired
    // location. This is a small convenience to keep all main routes together here on this file.
    { path: 'signed-in-redirect', pathMatch: 'full', redirectTo: 'brand' },

    // Auth routes for guests
    {
        path: '',
        canMatch: [NoAuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            { path: 'confirmation-required', loadChildren: () => import('app/modules/auth/confirmation-required/confirmation-required.module').then(m => m.AuthConfirmationRequiredModule) },
            { path: 'forgot-password', loadChildren: () => import('app/modules/auth/forgot-password/forgot-password.module').then(m => m.AuthForgotPasswordModule) },
            { path: 'reset-password', loadChildren: () => import('app/modules/auth/reset-password/reset-password.module').then(m => m.AuthResetPasswordModule) },
            { path: 'sign-in', loadChildren: () => import('app/modules/auth/sign-in/sign-in.module').then(m => m.AuthSignInModule) },
            { path: 'sign-up', loadChildren: () => import('app/modules/auth/sign-up/sign-up.module').then(m => m.AuthSignUpModule) },
            { path: 'unlock-session', loadChildren: () => import('app/modules/auth/unlock-session/unlock-session.module').then(m => m.AuthUnlockSessionModule) }

        ]
    },

    // Auth routes for authenticated users
    {
        path: '',
        canActivate: [AuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            { path: 'sign-out', loadChildren: () => import('app/modules/auth/sign-out/sign-out.module').then(m => m.AuthSignOutModule) },
        ]
    },

    // Landing routes
    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            { path: 'home', loadChildren: () => import('app/modules/landing/home/home.module').then(m => m.LandingHomeModule) },
        ]
    },

    // Admin routes
    {
        path: '',
        canActivate: [AuthGuard],
        component: LayoutComponent,
        resolve: {
            initialData: InitialDataResolver,
        },
        children: [
            // {path: 'example', loadChildren: () => import('app/modules/admin/example/example.module').then(m => m.ExampleModule)},
            { path: 'brand', loadChildren: () => import('app/modules/Brand/brand.module').then(m => m.BrandModule) },
            { path: 'posts', loadChildren: () => import('app/modules/posts/posts.module').then(m => m.PostsModule) },
            { path: 'analytics', loadChildren: () => import('app/modules/analytics/analytics.module').then(m => m.AnalyticsModule) },
            { path: 'settings', loadChildren: () => import('app/modules/settings/settings.module').then(m => m.SettingsModule) },
            { path: 'help-center', loadChildren: () => import('app/modules/help-center/help-center.module').then(m => m.HelpCenterModule) },
            { path: 'activity', loadChildren: () => import('app/modules/activities/activities.module').then(m => m.ActivitiesModule) },
            { path: 'insights', loadChildren: () => import('app/modules/insights/insights.module').then(m => m.InsightsModule) },
            { path: 'tasks', loadChildren: () => import('app/modules/tasks/tasks.module').then(m => m.TasksModule) },

        ]
    },
    {
        //   path: 'linkedincallback',
        path: 'callback',
        component: SocialChannelCallbackComponent
    },

    {
        path: 'facebookForGroupscallback',
        component: SocialChannelCallbackComponent
    },
    {
        path: 'instagramcallback',
        component: SocialChannelCallbackComponent
    },

    // {
    //     path: 'callback',
    //     component: InstagramloginComponent
    // },
    {
        path: 'facebookcallback',
        component: SocialChannelCallbackComponent
    },


    {
        path: 'twittercallback',
        component: SocialChannelCallbackComponent
    },
    // {
    //     path: '',
    //     canActivate: [AuthGuard],
    //     component: LayoutComponent,
    //     resolve: {
    //         initialData: InitialDataResolver,
    //     },
    //     children: [
    // {
    //     path: 'Posts',
    //     component: PostsListComponent,
    
    //   },]},
    //   {
    //     path: '',
    //     canActivate: [AuthGuard],
    //     component: LayoutComponent,
    //     resolve: {
    //         initialData: InitialDataResolver,
    //     },
    //     children: [
    // {
    //     path: 'post-details',
    //     component: PostDetailsComponent,
    
    //   },]}
];
