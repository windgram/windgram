import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TINYMCE_CONFIG_DEFAULT } from '../../tinymce/tinymce-config';
import { PostStatusType, PostType } from '../shared/posts.enum';
import { PostViewModel } from '../shared/posts.model';
import { PostsService } from '../shared/posts.service';

@Component({
  selector: 'app-posts-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class PostsEditComponent implements OnInit {
  id: number;
  form: FormGroup;
  tinymceConfig = TINYMCE_CONFIG_DEFAULT;
  posting = false;
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private postsService: PostsService
  ) {
    this.route.paramMap.subscribe(q => {
      if (q.has('id')) {
        this.id = Number(q.get('id'));
      }
    });
  }

  ngOnInit() {
    if (this.id) {
      this.postsService.get(this.id)
        .subscribe(res => {
          this.buildForm(res);
        });
    } else {
      this.buildForm();
    }
  }

  private buildForm(model?: PostViewModel) {
    this.form = this.fb.group({
      prentPostId: this.fb.control(model ? model.prentPostId : 0),
      title: this.fb.control(model ? model.title : ''),
      slug: this.fb.control(model ? model.slug : ''),
      description: this.fb.control(model ? model.description : ''),
      tags: this.fb.control(model ? model.tags : null),
      postType: this.fb.control(model ? model.postType : PostType.Public),
      postStatus: this.fb.control(model ? model.postStatus : PostStatusType.Draft),
      postContent: this.fb.group({
        metaDescription: this.fb.control(model ? model.metaDescription : ''),
        metaKeyword: this.fb.control(model ? model.metaKeyword : ''),
        htmlContent: this.fb.control(model ? model.htmlContent : '')
      }),
    });
  }

  onSubmit() {
    if (this.posting || this.form.invalid) {
      return;
    }
    this.postsService.post(this.form.value)
      .toPromise()
      .then(() => {

      })
      .finally(() => this.posting = false);
  }
}
