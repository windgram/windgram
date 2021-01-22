using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Windgram.Identity.STS.Models.Account
{
    public class EmailSendCodeViewModel
    {
        public string Email { get; set; }
    }
    public class PhoneNumberSendCodeViewModel
    {
        public string PhoneNumber { get; set; }
    }
}
