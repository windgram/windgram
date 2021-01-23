using IdentityModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windgram.Identity.ApplicationCore.Constants;
using Windgram.Identity.ApplicationCore.Domain.Entities;

namespace Windgram.Identity.ApplicationCore.ViewModels.Identity
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel() { }
        public UserClaimsViewModel(IEnumerable<UserIdentityUserClaim> claims)
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
                            if (!claim.ClaimValue.IsNullOrEmpty())
                            {
                                DateTime.TryParse(claim.ClaimValue, out var birthDate);
                                this.BirthDate = birthDate;
                            }
                            break;
                        case JwtClaimTypes.Gender:
                            this.Gender = claim.ClaimValue;
                            break;
                        case JwtClaimTypes.ZoneInfo:
                            this.ZoneInfo = claim.ClaimValue;
                            break;
                        case WindgramClaimTypes.Bio:
                            this.Bio = claim.ClaimValue;
                            break;
                    }
                });
            }
        }

        [JsonProperty(JwtClaimTypes.NickName)]
        public string NickName { get; set; }
        [JsonProperty(JwtClaimTypes.Picture)]
        public string Picture { get; set; }
        [JsonProperty(JwtClaimTypes.BirthDate)]
        public DateTime? BirthDate { get; set; }
        [JsonProperty(JwtClaimTypes.Gender)]
        public string Gender { get; set; }
        [JsonProperty(JwtClaimTypes.ZoneInfo)]
        public string ZoneInfo { get; set; }
        [JsonProperty(WindgramClaimTypes.Bio)]
        public string Bio { get; set; }
    }
}
