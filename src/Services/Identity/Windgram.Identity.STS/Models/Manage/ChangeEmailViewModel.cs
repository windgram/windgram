using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Manage
{
    public class ChangeEmailSendCodeInputModel
    {
        [Required]
        public string Email { get; set; }
    }
    public class ChangeEmailViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
