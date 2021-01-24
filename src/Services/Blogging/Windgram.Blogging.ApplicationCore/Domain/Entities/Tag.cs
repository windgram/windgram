using System;
using Windgram.Shared.Domain;

namespace Windgram.Blogging.ApplicationCore.Domain.Entities
{
    public class Tag : Entity<int>
    {
        public static string GetPublishedCacheKey => "tags_published";
        public static string GetByAliasCacheKey(string alias) => $"tag_by_alias_{alias}";
        public static string GetByNameCacheKey(string name) => $"tag_by_name_{name}";
        public string Alias { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
