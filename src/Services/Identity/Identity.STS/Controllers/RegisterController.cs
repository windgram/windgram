using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Windgram.Identity.STS.Models.Account;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Web.Shared.Services;

namespace Windgram.Identity.STS.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IBus _bus;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserContext _userContext;
        public RegisterController(
            ILogger<RegisterController> logger,
            IBus bus,
            UserManager<UserIdentity> userManager,
            IUserContext userContext
            )
        {
            _logger = logger;
            _bus = bus;
            _userManager = userManager;
            _userContext = userContext;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EmailRegisterViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(model);

            var user = new UserIdentity
            {
                UserName = Guid.NewGuid().ToString("N"),
                Email = model.Email,
                CreatedDateTime = DateTime.Now,
            };
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "此邮箱已经注册");
                return View(model);
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action(nameof(ConfirmEmailController.Callback), "ConfirmEmail", new { userId = user.Id, code, returnUrl }, HttpContext.Request.Scheme);

                //await _bus.Publish(new SendEmailIntegrationEvent()
                //{
                //    Body = $"感谢您注册西岸账号，点击<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>此处</a>验证你的邮箱！",
                //    IpAddress = _userContext.IpAddress,
                //    IsBodyHtml = true,
                //    Subject = "邮箱验证 - 西岸",
                //    To = user.Email
                //});
                //return RedirectToAction(nameof(ConfirmEmailController.Index), "ConfirmEmail", new
                //{
                //    ReturnUrl = returnUrl,
                //    UserId = user.Id
                //});
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
