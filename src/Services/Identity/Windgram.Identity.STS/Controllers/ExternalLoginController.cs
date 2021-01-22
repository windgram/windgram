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
using Windgram.Identity.STS.Extensions;
using Windgram.Identity.STS.Models.Account;
using Windgram.Shared.Application.IntegrationEvents;
using Windgram.Shared.Web.Services;

namespace Windgram.Identity.STS.Controllers
{
    public class ExternalLoginController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer _localizer;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        public ExternalLoginController(
            ILogger<ExternalLoginController> logger,
            IEventBus eventBus,
            IUserContext userContext,
            IStringLocalizer<ExternalLoginController> localizer,
            UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> signInManager)
        {
            _logger = logger;
            _eventBus = eventBus;
            _userContext = userContext;
            _localizer = localizer;
            _userManager = userManager;
            _signInManager = signInManager;
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
            var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user != null)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(loginInfo);
            }
            var result = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false, true);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(AccountController.LoginWith2fa), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(AccountController.Lockout), "Account");
            }
            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = loginInfo.LoginProvider;
            return View(nameof(Confirmation), new ExternalLoginConfirmationViewModel
            {
                Email = loginInfo.TryGetExternalLoginEmail()
            });
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
                if (user == null) // Bind external Login
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(nameof(model.Email), "");
                    }
                    else
                    {
                        var verified = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, UserIdentity.VerifyUserEmailTokenPurpose, model.Code);
                        if (verified)
                        {
                            var addResult = await _userManager.AddLoginAsync(user, loginInfo);
                            if (!addResult.Succeeded)
                            {
                                AddIdentityErrors(addResult);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(model.Code), "Invaild verify code");
                        }
                    }
                }
                else // Confirm email
                {
                    var verified = await _userManager.ConfirmEmailAsync(user, model.Code);
                    if (!verified.Succeeded)
                        ModelState.AddModelError(nameof(model.Code), "Invaild verify code");
                }
                if (ModelState.IsValid)
                {
                    await _signInManager.UpdateExternalAuthenticationTokensAsync(loginInfo);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
            }
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateEmailVerifyCode([FromBody] ExternalLoginBindEmailViewModel model)
        {
            // Get the information about the user from the external login provider
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return View("LoginFailure");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    user = await CreateUser(loginInfo, model.Email);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    _logger.LogInformation($"Email confirmation code {code}");
                    await _eventBus.Publish(new SendEmailIntegrationEvent
                    {
                        Body = $"感谢注册，您的验证码是：{code}",
                        IpAddress = _userContext.IpAddress,
                        IsBodyHtml = true,
                        Subject = "邮箱验证 - Windgram",
                        To = user.Email
                    });
                }
                else
                {
                    var code = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultEmailProvider, UserIdentity.VerifyUserEmailTokenPurpose);
                    _logger.LogInformation($"Email authenticator code is {code}");
                    await _eventBus.Publish(new SendEmailIntegrationEvent
                    {
                        Body = $"您的账号正在绑定外部登录，验证码：{code}",
                        IpAddress = _userContext.IpAddress,
                        IsBodyHtml = true,
                        Subject = "邮箱验证 - Windgram",
                        To = user.Email
                    });
                }
            }
            return BadRequest(ModelState);
        }
        private async Task<UserIdentity> CreateUser(ExternalLoginInfo loginInfo, string email)
        {
            var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user == null)
            {
                user = new UserIdentity()
                {
                    UserName = UserIdentity.GenerateGuidUserName(),
                    CreatedDateTime = DateTime.Now,
                    Email = email
                };
                var userResult = await _userManager.CreateAsync(user);
                if (!userResult.Succeeded)
                    throw new Exception(userResult.Errors.First().Description);
                var filtered = new List<Claim>();
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
