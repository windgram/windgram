import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PostDto, PostQueryModel, PostViewModel } from './posts.model';

const POSTS_URL = `${environment.bloggingUrl}/api/posts`;
@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(
    private httpClient: HttpClient
  ) { }

  get(id: number) {
    return this.httpClient.get<PostViewModel>(`${POSTS_URL}/${id}`);
  }

  post(dto: PostDto) {
    return this.httpClient.post<PostViewModel>(POSTS_URL, dto);
  }

  update(id: number, dto: PostDto) {
    return this.httpClient.put<void>(`${POSTS_URL}/${id}`, dto);
  }

  delete(id: number) {
    return this.httpClient.delete<void>(`${POSTS_URL}/${id}`);
  }

  list(query: PostQueryModel | any) {
    if (!query.keywords) {
      delete query.keywords;
    }
    if (!query.tag) {
      delete query.tag;
    }
    if (!query.status) {
      delete query.status;
    }
    return this.httpClient.get<PagedResult<PostViewModel>>(POSTS_URL, {
      params: query
    });
  }
}
