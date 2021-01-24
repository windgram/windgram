﻿using System;
using System.Collections.Generic;
using Windgram.Blogging.ApplicationCore.Enums;

namespace Windgram.Blogging.ApplicationCore.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public int ParentPostId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string CoverFileId { get; set; }
        public string CreatedBy { get; set; }
        public PostType PostType { get; set; }
        public PostStatusType PostStatus { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string HtmlContent { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public List<string> Tags { get; set; }
    }
}
