using Windgram.Identity.ApplicationCore.ViewModels.Identity;

namespace Windgram.Identity.API.ViewModels.Profile
{
    public class UserProfileViewModel : UserViewModel
    {
        public UserClaimsViewModel Profile { get; set; }
    }
}
