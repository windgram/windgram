using Microsoft.AspNetCore.Identity;
using System;
using Windgram.Shared.Domain;

namespace Windgram.Identity.ApplicationCore.Domain.Entities
{
    public class UserIdentityUserLogin : IdentityUserLogin<string>, IEntity
    {
        public DateTime CreatedDateTime { get; set; }
    }
}