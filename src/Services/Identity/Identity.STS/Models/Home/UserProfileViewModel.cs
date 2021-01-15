using System;
using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Home
{
    public class UserProfileViewModel
    {
        [Display(Name = "头像")]
        public string AvatarUrl { get; set; }

        [Display(Name = "邮箱地址")]
        public string Email { get; set; }
        [Display(Name = "昵称")]
        public string NickName { get; set; }
        [MaxLength(255)]
        [Display(Name = "简介")]
        public string Bio { get; set; }

        [Display(Name = "性别")]
        public string Gender { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        public DateTimeOffset? BirthDate { get; set; }

        [MaxLength(255)]
        [Display(Name = "城市")]
        public string City { get; set; }

        [MaxLength(255)]
        [Display(Name = "省份")]
        public string Province { get; set; }

        [MaxLength(255)]
        [Display(Name = "国家")]
        public string Country { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string StatusMessage { get; set; }
    }
}
