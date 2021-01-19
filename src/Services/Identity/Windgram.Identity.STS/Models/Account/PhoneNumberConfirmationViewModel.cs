namespace Windgram.Identity.STS.Models.Account
{
    public class PhoneNumberConfirmationViewModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
    }
}
