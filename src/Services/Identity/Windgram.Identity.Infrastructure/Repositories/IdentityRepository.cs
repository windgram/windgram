using Windgram.Shared.Domain;
using Windgram.Identity.ApplicationCore.Interfaces;

namespace Windgram.Identity.Infrastructure.Repositories
{
    public class IdentityRepository<TEntity> : EfRepository<TEntity>, IIdentityRepository<TEntity> where TEntity : class, IEntity
    {
        public IdentityRepository(IdentityContext context) : base(context)
        {

        }
    }
}
