﻿using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Account
{
    public class RegisterInputModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
