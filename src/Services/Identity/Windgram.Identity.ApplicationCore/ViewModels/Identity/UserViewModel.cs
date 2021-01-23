using IdentityModel;
using Newtonsoft.Json;
using System;
using Windgram.Identity.ApplicationCore.Constants;

namespace Windgram.Identity.ApplicationCore.ViewModels.Identity
{
    public class UserViewModel
    {
        [JsonProperty(JwtClaimTypes.Id)]
        public string Id { get; set; }

        [JsonProperty(JwtClaimTypes.Email)]
        public string Email { get; set; }
        [JsonProperty(JwtClaimTypes.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [JsonProperty(JwtClaimTypes.Name)]
        public string UserName { get; set; }

        [JsonProperty(WindgramClaimTypes.CreatedAt)]
        public DateTime CreatedDateTime { get; set; }
    }
}
