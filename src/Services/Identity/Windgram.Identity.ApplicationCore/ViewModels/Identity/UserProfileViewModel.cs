using IdentityModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windgram.Identity.ApplicationCore.Domain.Entities;

namespace Windgram.Identity.ApplicationCore.ViewModels.Identity
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel() { }
        public UserProfileViewModel(IEnumerable<UserIdentityUserClaim> claims)
        {
            if (claims != null)
            {
                claims.ToList().ForEach(claim =>
                {
                    switch (claim.ClaimType)
                    {
                        case JwtClaimTypes.NickName:
                            this.NickName = claim.ClaimValue;
                            break;
                        case JwtClaimTypes.Picture:
                            this.Picture = claim.ClaimValue;
                            break;
                        case JwtClaimTypes.BirthDate:
                            this.BirthDate = claim.ClaimValue;
                            break;
                        case JwtClaimTypes.Gender:
                            this.Gender = claim.ClaimValue;
                            break;
                        case JwtClaimTypes.ZoneInfo:
                            this.ZoneInfo = claim.ClaimValue;
                            break;
                    }
                });
            }
        }
        [JsonProperty("sub")]
        public string Id { get; set; }
        public string Email { get; set; }
        [JsonProperty("name")]
        public string UserName { get; set; }
        [JsonProperty("nickname")]
        public string NickName { get; set; }
        public string Picture { get; set; }
        [JsonProperty("birthdate")]
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        [JsonProperty("zoneinfo")]
        public string ZoneInfo { get; set; }
        [JsonProperty("updated_at")]
        public DateTime CreatedDateTime { get; set; }
    }
}
