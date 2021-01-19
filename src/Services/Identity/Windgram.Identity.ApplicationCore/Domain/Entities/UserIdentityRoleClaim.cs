using Microsoft.AspNetCore.Identity;
using Windgram.Shared.Domain;

namespace Windgram.Identity.ApplicationCore.Domain.Entities
{
    public class UserIdentityRoleClaim : IdentityRoleClaim<string>, IEntity
    {

    }
}