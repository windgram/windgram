using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class PostCommentEntityTypeConfiguration : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.ToTable(BloggingTableDefaults.PostComment, BloggingTableDefaults.Schema);
            builder.HasOne(x => x.Post).WithMany(p => p.PostComments).HasForeignKey(x => x.PostId);
        }
    }
}
