using Windgram.Shared.Domain;

namespace Windgram.Blogging.ApplicationCore.Domain.Entities
{
    public class PostTag : Entity<int>
    {
        public int TagId { get; set; }
        public int PostId { get; set; }
        public Tag Tag { get; set; }
        public Post Post { get; set; }
    }
}
