import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostsDetailsComponent } from './details/details.component';
import { PostsListComponent } from './list/list.component';
import { PostsComponent } from './posts.component';

const routes: Routes = [
  {
    path     : '',
    component: PostsComponent,
    children : [
      {
          path     : '',
          component: PostsListComponent
      },
      {
          path     : ':id',
          component: PostsDetailsComponent
      }
  ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PostsRoutingModule { }
