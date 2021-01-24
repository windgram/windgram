using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Windgram.Blogging.ApplicationCore.Constants;
using Windgram.Blogging.ApplicationCore.Domain.Entities;

namespace Windgram.Blogging.Infrastructure.EntityTypeConfigurations
{
    public class PostRatingEntityTypeConfiguration : IEntityTypeConfiguration<PostRating>
    {
        public void Configure(EntityTypeBuilder<PostRating> builder)
        {
            builder.ToTable(BloggingTableDefaults.PostRating, BloggingTableDefaults.Schema);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(64);
            builder.HasOne(x => x.Post).WithMany(p => p.PostRatings).HasForeignKey(x => x.PostId);
        }
    }
}
