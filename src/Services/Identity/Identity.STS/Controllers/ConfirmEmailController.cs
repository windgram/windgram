using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Windgram.Identity.STS.Models.Account;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Web.Shared.Services;
using Windgram.Application.Shared.IntegrationEvents;

namespace Windgram.Identity.STS.Controllers
{
    public class ConfirmEmailController : Controller
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserContext _userContext;
        private readonly IBus _bus;
        public ConfirmEmailController(
            UserManager<UserIdentity> userManager,
            IUserContext userContext,
            IBus bus)
        {
            _userManager = userManager;
            _userContext = userContext;
            _bus = bus;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string userId, string returnUrl)
        {
            var model = new EmailConfirmationViewModel
            {
                ReturnUrl = returnUrl,
                UserId = userId
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Index(EmailConfirmationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.UserId.IsNullOrEmpty())
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return View("Error");
            }
            if (user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "邮箱已经验证，无需再次验证");
                return View(model);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ConfirmEmailController.Callback), "ConfirmEmail", new { userId = user.Id, code, model.ReturnUrl }, HttpContext.Request.Scheme);

            await _bus.Publish(new SendEmailIntegrationEvent()
            {
                Body = $"你正在进行账号验证，点击<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>此处</a>验证你的邮箱！",
                IpAddress = _userContext.IpAddress,
                IsBodyHtml = true,
                Subject = "账号验证 - 西岸",
                To = user.Email
            });
            ViewData["StatusMessage"] = "验证邮件已经发送，请查看您的邮箱。注意：若未收到请检查是否被识别为垃圾邮件！";
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Callback(string userId, string code, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return View(result.Succeeded ? "Callback" : "Error");
        }
    }
}
