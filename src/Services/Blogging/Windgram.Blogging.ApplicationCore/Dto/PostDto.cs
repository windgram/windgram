using System.Collections.Generic;
using Windgram.Blogging.ApplicationCore.Enums;

namespace Windgram.Blogging.ApplicationCore.Dto
{
    public class PostDto
    {
        public int ParentPostId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string CoverFileId { get; set; }
        public PostType PostType { get; set; }
        public PostStatusType PostStatus { get; set; }
        public PostContentDto PostContent { get; set; }
        public List<string> Tags { get; set; }
    }
}
