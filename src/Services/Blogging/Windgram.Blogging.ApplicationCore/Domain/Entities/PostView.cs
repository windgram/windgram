using System;
using Windgram.Shared.Domain;

namespace Windgram.Blogging.ApplicationCore.Domain.Entities
{
    public class PostView : Entity<long>
    {
        public int PostId { get; set; }
        public string HostAddress { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Post Post { get; set; }
    }
}
