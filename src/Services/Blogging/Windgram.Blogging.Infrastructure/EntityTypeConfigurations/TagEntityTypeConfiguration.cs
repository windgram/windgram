using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(BloggingTableDefaults.Tag, BloggingTableDefaults.Schema);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(32);
            builder.Property(x => x.Alias).IsRequired(true).HasMaxLength(32);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(64);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(256);
            builder.HasIndex(x => x.Alias).IsUnique();
        }
    }
}
