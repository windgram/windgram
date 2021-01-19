using Microsoft.AspNetCore.Identity;
using System;
using Windgram.Shared.Domain;

namespace Windgram.Identity.ApplicationCore.Domain.Entities
{
    public class UserIdentity : IdentityUser, IEntity
    {
        public static string GetProfileById(string userId) => $"user_id_{userId}";
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}