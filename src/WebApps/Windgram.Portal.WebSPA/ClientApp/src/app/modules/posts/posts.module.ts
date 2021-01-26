import { NgModule } from '@angular/core';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { SharedModule } from 'src/app/shared/shared.module';
import { TinymceModule } from '../tinymce/tinymce.module';
import { POSTS_ROUTED_COMCOMPONENTS, PostsRoutingModule } from './posts-routing.module';
import { PostsService } from './shared/posts.service';

@NgModule({
  imports: [
    SharedModule,
    NzSelectModule,
    TinymceModule,
    PostsRoutingModule
  ],
  declarations: [
    POSTS_ROUTED_COMCOMPONENTS
  ],
  providers: [
    PostsService
  ]
})
export class PostsModule { }
