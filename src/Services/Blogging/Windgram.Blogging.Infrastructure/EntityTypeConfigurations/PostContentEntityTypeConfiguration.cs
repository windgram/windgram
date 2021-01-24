using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class PostContentEntityTypeConfiguration : IEntityTypeConfiguration<PostContent>
    {
        public void Configure(EntityTypeBuilder<PostContent> builder)
        {
            builder.ToTable(BloggingTableDefaults.PostContent, BloggingTableDefaults.Schema);
            builder.HasKey(x => x.PostId);
            builder.Property(x => x.MetaKeyword).IsRequired(false).HasMaxLength(256);
            builder.Property(x => x.MetaDescription).IsRequired(false).HasMaxLength(512);
            builder.HasOne(x => x.Post).WithOne(p => p.PostContent).HasForeignKey<PostContent>(x => x.PostId);
        }
    }
}
