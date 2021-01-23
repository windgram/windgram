using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windgram.Caching;
using Windgram.EventBus;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Identity.ApplicationCore.Queries;
using Windgram.Identity.STS.Models.Manage;
using Windgram.Shared.Application.IntegrationEvents;
using Windgram.Shared.Web.Services;

namespace Windgram.Identity.STS.Controllers
{
    public class ManageController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IStringLocalizer _localizer;
        private readonly IUserContext _userContext;
        private readonly IUserQueries _userQueries;
        private readonly IEventBus _eventBus;
        private readonly ICacheManager _cacheManager;
        private readonly UserManager<UserIdentity> _userManager;


        [TempData]
        public string StatusMessage { get; set; }
        public ManageController(
            ILogger<ManageController> logger,
            IStringLocalizer<ManageController> stringLocalizer,
            IUserContext userContext,
            IUserQueries userQueries,
            IEventBus eventBus,
            ICacheManager cacheManager,
            UserManager<UserIdentity> userManager)
        {
            _logger = logger;
            _localizer = stringLocalizer;
            _userContext = userContext;
            _userQueries = userQueries;
            _eventBus = eventBus;
            _cacheManager = cacheManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!StatusMessage.IsNullOrEmpty())
            {
                ViewData["StatusMessage"] = StatusMessage;
            }
            var vm = await _userQueries.GetUserById(_userContext.UserId);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var vm = await _userQueries.GetUserClaimsById(_userContext.UserId);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileInputModel model)
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserName()
        {
            var user = await _userQueries.GetUserById(_userContext.UserId);
            ViewData["CurrentUserName"] = user.UserName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserName(ChangeUserNameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(_userContext.UserId);
                var result = await _userManager.SetUserNameAsync(user, model.UserName);
                if (result.Succeeded)
                {
                    this.StatusMessage = _localizer["UserNameChanged"];
                    await _cacheManager.RemoveAsync(UserIdentity.GetById(user.Id));
                    return RedirectToAction(nameof(Index));
                }
                AddIdentityErrors(result);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Email()
        {
            var user = await _userQueries.GetUserById(_userContext.UserId);
            ViewData["CurrentEmail"] = user.Email;
            return View();
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
                    this.StatusMessage = _localizer["EmailChanged"];
                    await _cacheManager.RemoveAsync(UserIdentity.GetById(user.Id));
                    return RedirectToAction(nameof(Index));
                }
                AddIdentityErrors(result);

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateEmailVerifyCode([FromBody] ChangeEmailSendCodeInputModel model)
        {
            var user = await _userManager.FindByIdAsync(_userContext.UserId);
            if (user == null)
                return BadRequest();
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            _logger.LogInformation($"Change Email code is {code}");
            await _eventBus.Publish(new SendEmailIntegrationEvent
            {
                Body = $"您正在修改登录邮箱，验证码是：{code}",
                IpAddress = _userContext.IpAddress,
                IsBodyHtml = true,
                Subject = "修改登录邮箱 - Windgram",
                To = user.Email
            });
            return Ok();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(_userContext.UserId);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    this.StatusMessage = _localizer["PasswordChanged"];
                    return RedirectToAction(nameof(Index));
                }
                AddIdentityErrors(result);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await _userManager.FindByIdAsync(_userContext.UserId);
            var logins = await _userManager.GetLoginsAsync(user);
            return View(logins.Select(x => new UserLoginInfoViewModel
            {
                LoginProvider = x.LoginProvider,
                ProviderDisplayName = x.ProviderDisplayName,
                ProviderKey = x.ProviderKey
            }));
        }
    }
}