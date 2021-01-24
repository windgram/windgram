using System;

namespace Windgram.Blogging.ApplicationCore.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
