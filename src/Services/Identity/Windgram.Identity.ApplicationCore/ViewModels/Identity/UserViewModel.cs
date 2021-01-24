using System;

namespace Windgram.Identity.ApplicationCore.ViewModels.Identity
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
