using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Identity.ApplicationCore.Queries;
using Windgram.Identity.ApplicationCore.ViewModels.Identity;
using Windgram.Identity.STS.Models.Profile;
using Windgram.Shared.Web.Services;

namespace Windgram.Identity.STS.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUserContext _userContext;
        private readonly IUserQueries _userQueries;
        private readonly UserManager<UserIdentity> _userManager;
        public ProfileController(IUserContext userContext, IUserQueries userQueries, UserManager<UserIdentity> userManager)
        {
            _userContext = userContext;
            _userQueries = userQueries;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _userQueries.GetUserProfileById(_userContext.UserId);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Email(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(_userContext.UserId);
                var result = await _userManager.ChangeEmailAsync(user, model.Email, model.Code);
                if (result.Succeeded)
                {
                    return View(model);
                }
                AddIdentityErrors(result);

            }
            return View(model);
        }
    }
}
