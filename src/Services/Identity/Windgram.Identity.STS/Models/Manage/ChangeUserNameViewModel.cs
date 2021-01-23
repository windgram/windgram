using System.ComponentModel.DataAnnotations;

namespace Windgram.Identity.STS.Models.Manage
{
    public class ChangeUserNameViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
