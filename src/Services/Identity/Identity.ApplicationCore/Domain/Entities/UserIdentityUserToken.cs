using Microsoft.AspNetCore.Identity;
using Windgram.Domain.Shared;

namespace Windgram.ApplicationCore.Domain.Entities
{
    public class UserIdentityUserToken : IdentityUserToken<string>, IEntity
    {

    }
}