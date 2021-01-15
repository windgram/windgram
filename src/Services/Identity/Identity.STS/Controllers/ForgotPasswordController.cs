using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Windgram.Identity.STS.Models.Account;
using Windgram.Web.Shared.Services;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.EventBus;
using Windgram.Application.Shared.IntegrationEvents;

namespace Windgram.Identity.STS.Controllers
{
    public class ForgotPasswordController : BaseController
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserContext _userContext;
        private readonly IEventBus _eventBus;
        public ForgotPasswordController(
            UserManager<UserIdentity> userManager,
            IUserContext userContext,
            IEventBus eventBus
            )
        {
            _userManager = userManager;
            _userContext = userContext;
            _eventBus = eventBus;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var vm = new ForgotPasswordViewModel();
            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserIdentity user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "未能找到注册信息");
                    return View(model);
                }
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "未能找到用户");
                    return View(model);
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPasswordController.Index), "ResetPassword", new { userId = user.Id, code }, HttpContext.Request.Scheme);

                await _eventBus.Publish(new SendEmailIntegrationEvent()
                {
                    Body = $"你正在找回密码，点击<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>此处</a>设置新密码！",
                    IpAddress = _userContext.IpAddress,
                    IsBodyHtml = true,
                    Subject = "重置密码 - 西岸",
                    To = user.Email
                });
                return View("Confirmation");
            }

            return View(model);
        }
    }
}
