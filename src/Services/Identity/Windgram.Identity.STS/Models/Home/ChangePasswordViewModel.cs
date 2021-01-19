using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Home
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "原密码")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string StatusMessage { get; set; }
    }
}
