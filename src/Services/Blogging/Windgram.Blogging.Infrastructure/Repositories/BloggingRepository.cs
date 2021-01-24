using Windgram.Shared.Domain;

namespace Windgram.Blogging.Infrastructure.Repositories
{
    public class BloggingRepository<TEntity> : EfRepository<TEntity> where TEntity : Entity
    {
        public BloggingRepository(BloggingDbContext context) : base(context)
        {
        }
    }
}
