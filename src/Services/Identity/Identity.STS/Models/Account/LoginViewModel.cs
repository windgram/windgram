using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Windgram.Identity.STS.Models.Account
{
    public class LoginInputModel
    {
        [Display(Name = "邮箱地址")]
        public string UserName { get; set; }
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "记住密码")]
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    }
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string TwoFactorCode { get; set; }
        public bool RememberMachine { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
