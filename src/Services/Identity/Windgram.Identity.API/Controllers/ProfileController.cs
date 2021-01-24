using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Windgram.Identity.API.ViewModels.Profile;
using Windgram.Identity.ApplicationCore.Queries;
using Windgram.Shared.Web.Services;

namespace Windgram.Identity.API.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUserContext _userContext;
        private readonly IUserQueries _userQueries;
        public ProfileController(
            IUserContext userContext,
            IUserQueries userQueries)
        {
            _userContext = userContext;
            _userQueries = userQueries;
        }
        [HttpGet]
        public async Task<ActionResult<UserProfileViewModel>> Get()
        {
            var user = await _userQueries.GetUserById(_userContext.UserId);
            if (user == null)
                return NoContent();
            var claims = await _userQueries.GetUserClaimsById(_userContext.UserId);

            return Ok(new UserProfileViewModel
            {
                CreatedDateTime = user.CreatedDateTime,
                Email = user.Email,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                Profile = claims,
                UserName = user.UserName
            });
        }
    }
}
