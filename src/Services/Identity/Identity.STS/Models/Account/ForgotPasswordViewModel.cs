using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "邮箱地址")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { get; set; }
    }
}
