using Windgram.Shared.Domain;

namespace Windgram.Identity.ApplicationCore.Interfaces
{
    public interface IIdentityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
    }
}
