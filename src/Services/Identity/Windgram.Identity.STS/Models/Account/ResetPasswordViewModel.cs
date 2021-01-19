using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "新密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "确认新密码")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
