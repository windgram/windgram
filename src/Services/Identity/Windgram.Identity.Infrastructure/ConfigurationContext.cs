using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Windgram.Shared.Domain;
using Windgram.Shared.Domain.Extensions;

namespace Windgram.Identity.Infrastructure
{
    public class ConfigurationContext : ConfigurationDbContext<ConfigurationContext>, IUnitOfWork
    {
        private readonly IMediator _mediator;
        public ConfigurationContext(
            DbContextOptions<ConfigurationContext> options,
            ConfigurationStoreOptions storeOptions,
            IMediator mediator = null) : base(options, storeOptions)
        {
            _mediator = mediator;
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
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
  