using System.Threading.Tasks;
using Windgram.Identity.ApplicationCore.ViewModels.Identity;

namespace Windgram.Identity.ApplicationCore.Queries
{
    public interface IUserQueries
    {
        Task<UserProfileViewModel> GetUserProfileById(string id);
    }
}