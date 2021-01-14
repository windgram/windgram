using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windgram.Web.Shared.Services;

namespace Windgram.Identity.API.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUserContext _userContext;
        public ProfileController(IUserContext userContext)
        {
            _userContext = userContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                EmailAddress = _userContext.EmailAddress,
                IpAddress = _userContext.IpAddress,
                UserId = _userContext.UserId
            });
        }
    }
}
