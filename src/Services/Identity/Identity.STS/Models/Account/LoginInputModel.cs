using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Account
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
    }
}
