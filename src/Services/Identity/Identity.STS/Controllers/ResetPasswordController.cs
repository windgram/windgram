using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Windgram.Identity.STS.Models.Account;
using Windgram.Identity.ApplicationCore.Domain.Entities;

namespace Windgram.Identity.STS.Controllers
{
    public class ResetPasswordController : BaseController
    {
        private readonly UserManager<UserIdentity> _userManager;

        public ResetPasswordController(UserManager<UserIdentity> userManager)
        {
            _userManager = userManager;
        }
        [AllowAnonymous]
        public ActionResult Index(string userId, string token = null)
        {
            var vm = new ResetPasswordViewModel
            {
                Token = token,
                UserId = userId
            };
            return View(vm);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Confirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Confirmation");
            }
            AddErrors(result);
            return View(model);
        }
    }
}
