using System;

namespace Windgram.Identity.STS.Models.Manage
{
    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
