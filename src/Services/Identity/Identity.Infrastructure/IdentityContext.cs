using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Domain.Shared;
using Windgram.Domain.Shared.Extensions;
using Windgram.Identity.ApplicationCore.Constants;

namespace Windgram.Identity.Infrastructure
{
    public class IdentityContext : IdentityDbContext
       <UserIdentity,
       UserIdentityRole,
       string,
       UserIdentityUserClaim,
       UserIdentityUserRole,
       UserIdentityUserLogin,
       UserIdentityRoleClaim,
       UserIdentityUserToken>,
       IDbContext
    {
        private readonly IMediator _mediator;
        public IdentityContext(DbContextOptions<IdentityContext> options, IMediator mediator = null) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<UserIdentityRole>().ToTable(IdentityTableDefaults.Role, IdentityTableDefaults.Schema);
            builder.Entity<UserIdentityRoleClaim>().ToTable(IdentityTableDefaults.RoleClaim, IdentityTableDefaults.Schema);
            builder.Entity<UserIdentityUserRole>().ToTable(IdentityTableDefaults.UserRole, IdentityTableDefaults.Schema);
            builder.Entity<UserIdentity>().ToTable(IdentityTableDefaults.User, IdentityTableDefaults.Schema);
            builder.Entity<UserIdentityUserLogin>().ToTable(IdentityTableDefaults.UserLogin, IdentityTableDefaults.Schema);
            builder.Entity<UserIdentityUserClaim>().ToTable(IdentityTableDefaults.UserClaim, IdentityTableDefaults.Schema);
            builder.Entity<UserIdentityUserToken>().ToTable(IdentityTableDefaults.UserToken, IdentityTableDefaults.Schema);
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
