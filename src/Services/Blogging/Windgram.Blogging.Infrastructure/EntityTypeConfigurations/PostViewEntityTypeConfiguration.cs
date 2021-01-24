using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class PostViewEntityTypeConfiguration : IEntityTypeConfiguration<PostView>
    {
        public void Configure(EntityTypeBuilder<PostView> builder)
        {
            builder.ToTable(BloggingTableDefaults.PostView, BloggingTableDefaults.Schema);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(64);
            builder.Property(x => x.HostAddress).IsRequired(false).HasMaxLength(128);
            builder.HasOne(x => x.Post).WithMany(p => p.PostViews).HasForeignKey(x => x.PostId);
        }
    }
}
