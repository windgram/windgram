using Microsoft.AspNetCore.Identity;
using Windgram.Domain.Shared;

namespace Windgram.Identity.ApplicationCore.Domain.Entities
{
    public class UserIdentityRoleClaim : IdentityRoleClaim<string>, IEntity
    {

    }
}