import { PostType, PostStatusType } from './posts.enum';

export interface PostDto {
  prentPostId: number;
  title: string;
  slug: string;
  description: string;
  coverFileId: string;
  postType: PostType;
  postStatus: PostStatusType;
  postContent: PostContentDto;
  tags: string[];
}

export interface PostContentDto {
  metaDescription: string;
  metaKeyword: string;
  htmlContent: string;
}

export interface PostViewModel {
  id: number;
  prentPostId: number;
  title: string;
  slug: string;
  description: string;
  coverFileId: string;
  postType: PostType;
  postStatus: PostStatusType;
  metaDescription: string;
  metaKeyword: string;
  htmlContent: string;
  createdDateTime: string;
  publishedDateTime: string;
  tags: string[];
}

export interface PostQueryModel extends PagedQuery {
  keywords?: string;
  tag?: string;
  status?: PostStatusType;
}
