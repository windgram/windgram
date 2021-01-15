using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Account
{
    public class EmailRegisterViewModel
    {

        [Display(Name = "邮箱地址")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
