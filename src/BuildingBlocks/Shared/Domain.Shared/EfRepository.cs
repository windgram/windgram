using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Windgram.Domain.Shared
{
    public abstract class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IDbContext _context;
        private DbSet<TEntity> _entities;
        public EfRepository(IDbContext context)
        {
            _context = context;
        }
        public DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = this._context.Set<TEntity>();
                }
                return _entities;
            }
        }
        public IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public IUnitOfWork UnitOfWork => _context;

        public IQueryable<TEntity> Table => Entities;

        public void Add(TEntity entity)
        {

            Entities.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
        }

        public EntityEntry<TEntity> Entry(TEntity entity)
        {
            return _context.Entry(entity);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
        }
    }
}
