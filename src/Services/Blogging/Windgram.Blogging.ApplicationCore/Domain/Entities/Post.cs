using System;
using System.Collections.Generic;
using Windgram.Blogging.ApplicationCore.Enums;
using Windgram.Shared.Domain;

namespace Windgram.Blogging.ApplicationCore.Domain.Entities
{
    public class Post : AggregateRoot<int>
    {
        public static string GetBySlugCacheKey(string blog, string slug) => $"post_by_{blog}_{slug}";
        public static string GetByIdCacheKey(int id) => $"post_by_{id}";
        private ICollection<PostView> _postViews;
        private ICollection<PostRating> _postRatings;
        private ICollection<PostComment> _postComments;
        private ICollection<PostTag> _postTags;
        public int ParentPostId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string CoverFileId { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public PostType PostType { get; set; }
        public PostStatusType PostStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public PostContent PostContent { get; set; }
        public ICollection<PostTag> PostTags
        {
            get => _postTags ?? (_postTags = new List<PostTag>());
            private set => _postTags = value;
        }
        public ICollection<PostView> PostViews
        {
            get => _postViews ?? (_postViews = new List<PostView>());
            private set => _postViews = value;
        }
        public ICollection<PostRating> PostRatings
        {
            get => _postRatings ?? (_postRatings = new List<PostRating>());
            private set => _postRatings = value;
        }
        public ICollection<PostComment> PostComments
        {
            get => _postComments ?? (_postComments = new List<PostComment>());
            private set => _postComments = value;
        }
    }
}
