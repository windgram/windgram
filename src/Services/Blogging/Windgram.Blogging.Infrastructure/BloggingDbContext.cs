using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Windgram.Shared.Domain;
using Windgram.Shared.Domain.Extensions;

namespace Windgram.Blogging.Infrastructure
{
    public class BloggingDbContext : DbContext, IDbContext
    {
        private readonly IMediator _mediator;
        public BloggingDbContext(DbContextOptions<BloggingDbContext> options, IMediator mediator = null) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BloggingDbContext).Assembly);
        }

        public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            await base.SaveChangesAsync(cancellationToken);
            if (_mediator != null)
            {
                // Dispatch Domain Events collection. 
                // Choices:
                // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
                // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
                // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
                // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
                await _mediator.DispatchDomainEventsAsync(this);
            }
            return true;
        }
    }
}
