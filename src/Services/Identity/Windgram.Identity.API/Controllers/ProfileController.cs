using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windgram.Identity.ApplicationCore.Queries;
using Windgram.Identity.ApplicationCore.ViewModels.Identity;
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
        public async Task<ActionResult<UserClaimsViewModel>> Get()
        {
            var vm = await _userQueries.GetUserClaimsById(_userContext.UserId);
            if (vm == null)
                return NotFound();
            return Ok(vm);
        }
    }
}
