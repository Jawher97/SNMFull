import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { PostsDetailsComponent } from './details/details.component';
import { PostsListComponent } from './list/list.component';
import { PostsComponent } from './posts.component';
import { PostsHomeComponent } from './posts-home/posts-home.component';
import { LinkedinPostResolver } from 'app/core/resolvers/linkedin-post.resolver';
import { FacebookPosts } from 'app/core/resolvers/facebook-posts.resolver';
import { PostDetailsComponent } from './post-details/post-details.component';

const routes: Routes = [
  //   {

  //     path     : '',
  //     component: PostsComponent,
  //     children : [
  //       {
  //           path     : '',
  //           component: PostsListComponent
  //       },
  //       {
  //           path     : ':id',
  //           component: PostsDetailsComponent
  //       }
  //   ]
  // },
  // {
  //   path: '', pathMatch: 'full',
  //   resolve: {

  //     postsHome: LinkedinPostResolver
  //   }, redirectTo: 'posts-home'
  // },
  // {
  //   path: 'linkedin-posts',
  //   component: PostsListComponent,

  // },
  // {
  //   path: 'posts-home',
  //   component: PostsHomeComponent,
  //   resolve: {
  //     postsHome: LinkedinPostResolver,
  //     facebookPosts:FacebookPosts
  //   },
  //   children : [
  //     {
  //         path         : 'detail',
  //         // path         : ':id',
  //         component    : PostDetailsComponent,
  //         // resolve      : {
  //         //     contact  : ContactsContactResolver,
  //         //     countries: ContactsCountriesResolver
  //         // },
  //         // canDeactivate: [CanDeactivateContactsDetails]
  //     }
  // ]

  // },
  
 {
    path: 'Posts',
    component: PostsListComponent,

  },
//   {
//     path: ':displayName',
//     component: PostsHomeComponent,
//     resolve: {
//       // brand: BrandResolver,
//     },
//     children: [

//       {
//         path: '',
//         component: PostsHomeComponent,
//       },

//     ]
//   }
 ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PostsRoutingModule { }
