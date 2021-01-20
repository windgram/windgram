using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required] 
        public string Code { get; set; }
    }
    public class ExternalLoginBindEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
