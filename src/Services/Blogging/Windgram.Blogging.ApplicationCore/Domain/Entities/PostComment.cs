using Windgram.Shared.Domain;

namespace Windgram.Blogging.ApplicationCore.Domain.Entities
{
    public class PostComment : Entity<long>
    {
        public int PostId { get; set; }
        public long CommentId { get; set; }
        public bool IsApproved { get; set; }
        public Post Post { get; set; }
    }
}
