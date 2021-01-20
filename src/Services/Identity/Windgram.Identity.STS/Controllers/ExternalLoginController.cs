using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Windgram.EventBus;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Identity.STS.Models.Account;

namespace Windgram.Identity.STS.Controllers
{
    public class ExternalLoginController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IStringLocalizer _localizer;
        private readonly IEventBus _eventBus;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        public ExternalLoginController(
            ILogger<ExternalLoginController> logger,
            IEventBus eventBus,
            UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> signInManager,
            IStringLocalizer<ExternalLoginController> localizer)
        {
            _logger = logger;
            _eventBus = eventBus;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Challenge(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(Callback), new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, remoteError);

                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            var user = await AutoProvisionUser(loginInfo);
            // Sign in the user with this external login provider if the user already has a login.
            var props = new AuthenticationProperties();
            props.StoreTokens(loginInfo.AuthenticationTokens);
            props.IsPersistent = true;

            await _signInManager.SignInAsync(user, props);
          //  var result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent: true);
            //if (result.Succeeded)
            //{
            //    return RedirectToLocal(returnUrl);
            //}
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(AccountController.LoginWith2fa), "Account", new { ReturnUrl = returnUrl });
            //}
            //if (result.IsLockedOut)
            //{
            //    return View("Lockout");
            //}
            return RedirectToAction(nameof(Confirmation), new { returnUrl });
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Confirmation(string returnUrl = null)
        {
            var user = User;
            var h = HttpContext.GetTokenAsync("access_token");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return View("LoginFailure");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
                if (user == null)
                {
                    return View("LoginFailure");
                }
                var result = await _userManager.ChangeEmailAsync(user, model.Email, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToLocal(returnUrl);
                }
                AddIdentityErrors(result);
            }
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateChangeEmailCode(ExternalLoginBindEmailViewModel model)
        {
            // Get the information about the user from the external login provider
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return View("LoginFailure");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
                if (user == null)
                {
                    ModelState.AddModelError("", "LoginFailure");
                }
                else
                {
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                }
            }
            return BadRequest(ModelState);
        }

        private async Task<UserIdentity> AutoProvisionUser(ExternalLoginInfo loginInfo)
        {
            var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user == null)
            {
                user = new UserIdentity()
                {
                    UserName = UserIdentity.GenerateGuidUserName(),
                    CreatedDateTime = DateTime.Now
                };
                var userResult = await _userManager.CreateAsync(user);
                if (!userResult.Succeeded)
                    throw new Exception(userResult.Errors.First().Description);
                var filtered = new List<Claim>();
                var email = loginInfo.Principal.FindFirstValue(JwtClaimTypes.Email) ?? loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                if (!email.IsNullOrEmpty())
                {
                    filtered.Add(new Claim(JwtClaimTypes.Email, email));
                }
                var nickName = loginInfo.Principal.FindFirstValue(JwtClaimTypes.NickName) ??
                    loginInfo.Principal.FindFirstValue(JwtClaimTypes.Name) ??
                    loginInfo.Principal.FindFirstValue(ClaimTypes.Name);

                if (!nickName.IsNullOrEmpty())
                {
                    filtered.Add(new Claim(JwtClaimTypes.NickName, nickName));
                }

                var pictureUrl = loginInfo.Principal.FindFirstValue(JwtClaimTypes.Picture);
                if (!pictureUrl.IsNullOrEmpty())
                {
                    filtered.Add(new Claim(JwtClaimTypes.Picture, pictureUrl));
                }

                var claimsResult = await _userManager.AddClaimsAsync(user, filtered);
                if (!claimsResult.Succeeded)
                    throw new Exception(claimsResult.Errors.First().Description);
                var loginInfoResult = await _userManager.AddLoginAsync(user, loginInfo);
                if (!loginInfoResult.Succeeded)
                    throw new Exception(loginInfoResult.Errors.First().Description);
            }
            return user;
        }
    }
}
