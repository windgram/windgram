using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable(BloggingTableDefaults.Post, BloggingTableDefaults.Schema);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(64);
            builder.Property(x => x.Title).IsRequired(true).HasMaxLength(64);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(512);
            builder.Property(x => x.Slug).IsRequired(false).HasMaxLength(128);
            builder.Property(x => x.CoverFileId).IsRequired(false).HasMaxLength(128);
        }
    }
}
