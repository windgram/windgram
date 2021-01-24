using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class PostTagEntityTypeConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable(BloggingTableDefaults.PostTag, BloggingTableDefaults.Schema);
            builder.HasOne(x => x.Tag).WithMany().HasForeignKey(x => x.TagId);
            builder.HasOne(x => x.Post).WithMany(p => p.PostTags).HasForeignKey(x => x.PostId);
        }
    }
}
