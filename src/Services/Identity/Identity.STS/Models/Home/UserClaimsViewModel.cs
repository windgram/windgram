using System;

namespace Windgram.Identity.STS.Models.Home
{
    public class UserClaimsViewModel
    {
        public string Bio { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
    }
}
