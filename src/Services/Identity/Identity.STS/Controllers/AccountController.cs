using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Windgram.ApplicationCore.Domain.Entities;
using Windgram.Identity.STS.Models.Account;

namespace Windgram.Identity.STS.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IEventService _events;

        public AccountController(
            UserManager<UserIdentity> userManager,
            SignInManager<UserIdentity> signInManager,
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));
            }
            return Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            var existUser = await _userManager.FindByEmailAsync(model.Email);
            if (existUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "此邮箱已经注册");
                return BadRequest(ModelState);
            }
            var user = new UserIdentity
            {
                UserName = model.Email,
                Email = model.Email,
                CreatedDateTime = DateTime.Now,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
            return BadRequest(ModelState);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
