﻿using Microsoft.AspNetCore.Identity;
using System;
using Windgram.Domain.Shared;

namespace Windgram.ApplicationCore.Domain.Entities
{
    public class UserIdentityRole : IdentityRole, IEntity
    {
        public string DisplayName { get; set; }
        public DateTime Created { get; set; }
    }
}