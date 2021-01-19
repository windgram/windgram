using Microsoft.AspNetCore.Identity;
using System;
using Windgram.Shared.Domain;

namespace Windgram.Identity.ApplicationCore.Domain.Entities
{
    public class UserIdentityRole : IdentityRole, IEntity
    {
        public string DisplayName { get; set; }
        public DateTime Created { get; set; }
    }
}