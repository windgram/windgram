using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Windgram.EventBus;
using Windgram.Identity.ApplicationCore.Domain.Entities;

namespace Windgram.Identity.STS.Controllers
{
    [AllowAnonymous]
    public class ExternalLoginController : BaseController
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IEventBus _eventBus;
        public ExternalLoginController(
            UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            IEventBus eventBus)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _eventBus = eventBus;
        }
        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(Callback), "ExternalLogin", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        {
            var u = User;
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, remoteError);

                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            var avatarUrl = loginInfo.Principal.FindFirstValue(JwtClaimTypes.Picture);
            var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user == null)
            {
                var nickName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);
                var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                user = new UserIdentity
                {
                    UserName = Guid.NewGuid().ToString(),
                    CreatedDateTime = DateTime.Now,
                };
                if (!email.IsNullOrEmpty())
                {
                    var userByEmail = await _userManager.FindByEmailAsync(email);
                    if (userByEmail == null)
                    {
                        user.Email = email;
                    }
                }
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, loginInfo);
                }
                else
                {
                    AddErrors(result);
                    return RedirectToAction(nameof(LoginController.Index), "Login");
                }
            }
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToLocal(returnUrl);

        }
        //[HttpGet]
        //public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
        //{
        //    var u = User;
        //    if (remoteError != null)
        //    {
        //        ModelState.AddModelError(string.Empty, remoteError);

        //        return RedirectToAction(nameof(LoginController.Index), "Login");
        //    }
        //    var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction(nameof(LoginController.Index), "Login");
        //    }
        //    var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        //    if (user == null)
        //    {
        //        user = new UserIdentity
        //        {

        //        }
        //    }
        //    // Sign in the user with this external login provider if the user already has a login.
        //    var result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent: false);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.RequiresTwoFactor)
        //    {
        //        return RedirectToAction(nameof(LoginController.With2factor), "Login", new { ReturnUrl = returnUrl });
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return View("Lockout");
        //    }
        //    if (result.IsNotAllowed)
        //    {
        //        var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        //        return RedirectToAction(nameof(ConfirmEmailController.Index), "ConfirmEmail", new
        //        {
        //            ReturnUrl = returnUrl,
        //            UserId = user.Id
        //        });
        //    }

        //    // If the user does not have an account, then ask the user to create an account.
        //    ViewData["ReturnUrl"] = returnUrl;
        //    ViewData["LoginProvider"] = loginInfo.LoginProvider;
        //    var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);

        //    return View("Confirmation", new ExternalLoginConfirmationViewModel { Email = email });
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Confirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");

        //    // Get the information about the user from the external login provider
        //    var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return View("ExternalLoginFailure");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var existUser = await _userManager.FindByEmailAsync(model.Email);
        //        if (existUser != null)
        //        {
        //            ModelState.AddModelError(nameof(model.Email), "此邮箱已经注册");
        //            return View(model);
        //        }
        //        var user = new UserIdentity
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            CreatedDateTime = DateTimeOffset.Now
        //        };
        //        var avatarUrl = loginInfo.Principal.FindFirst(JwtClaimTypes.Picture)?.Value;
        //        if (!avatarUrl.IsNullOrEmpty())
        //        {
        //            user.AvatarFileId = Guid.NewGuid().ToString("N");

        //        }
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            if (!avatarUrl.IsNullOrEmpty())
        //            {
        //                await _bus.Publish(new DownloadToSaveFileIntegrationEvent(user.AvatarFileId, avatarUrl));
        //            }
        //            result = await _userManager.AddLoginAsync(user, loginInfo);
        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, additionalClaims: user.GetAdditionalClaims(_settings));

        //                return RedirectToLocal(returnUrl);
        //            }
        //        }

        //        AddErrors(result);
        //    }

        //    ViewData["LoginProvider"] = loginInfo.LoginProvider;
        //    ViewData["ReturnUrl"] = returnUrl;

        //    return View(model);
        //}

    }
}
