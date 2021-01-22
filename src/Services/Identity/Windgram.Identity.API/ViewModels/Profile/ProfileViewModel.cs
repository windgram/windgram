using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Windgram.Identity.API.ViewModels.Profile
{
    public class ProfileViewModel
    {
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
