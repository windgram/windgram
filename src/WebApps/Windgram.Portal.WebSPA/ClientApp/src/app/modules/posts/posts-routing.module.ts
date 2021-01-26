import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostsComponent } from './posts.component';
import { PostsEditComponent } from './edit/edit.component';
import { PostsListComponent } from './list/list.component';

const routes: Routes = [
  {
    path: '',
    component: PostsComponent,
    children: [
      {
        path: 'list',
        component: PostsListComponent
      },
      {
        path: 'new',
        component: PostsEditComponent
      },
      {
        path: 'edit/:id',
        component: PostsEditComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class PostsRoutingModule { }
export const POSTS_ROUTED_COMCOMPONENTS = [
  PostsComponent,
  PostsListComponent,
  PostsEditComponent
];

